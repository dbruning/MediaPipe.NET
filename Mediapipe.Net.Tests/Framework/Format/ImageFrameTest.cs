// Copyright (c) homuler and The Vignette Authors
// This file is part of MediaPipe.NET.
// MediaPipe.NET is licensed under the MIT License. See LICENSE for details.

using System;
using System.Linq;
using Mediapipe.Net.Core;
using Mediapipe.Net.Framework.Format;
using NUnit.Framework;

namespace Mediapipe.Net.Tests.Framework.Format
{
    public class ImageFrameTest
    {
        #region Constructor
        [Test, SignalAbort]
        public void Ctor_ShouldInstantiateImageFrame_When_CalledWithNoArguments()
        {
            using var imageFrame = new ImageFrame();
#pragma warning disable IDE0058
            Assert.AreEqual(imageFrame.Format, ImageFormat.Unknown);
            Assert.AreEqual(imageFrame.Width, 0);
            Assert.AreEqual(imageFrame.Height, 0);
            // As these are now properties, i had to ToString() them so that they are run.
            Assert.Throws<FormatException>(() => imageFrame.ChannelSize.ToString());
            Assert.Throws<FormatException>(() => imageFrame.NumberOfChannels.ToString());
            Assert.Throws<FormatException>(() => imageFrame.ByteDepth.ToString());
            Assert.AreEqual(imageFrame.WidthStep, 0);
            Assert.AreEqual(imageFrame.PixelDataSize, 0);
            Assert.Throws<FormatException>(() => imageFrame.PixelDataSizeStoredContiguously.ToString());
            Assert.True(imageFrame.IsEmpty);
            Assert.False(imageFrame.IsContiguous);
            Assert.False(imageFrame.IsAligned(16));
            unsafe
            {
                Assert.True(imageFrame.MutablePixelData == null);
            }
#pragma warning restore IDE0058
        }

        [Test]
        public void Ctor_ShouldInstantiateImageFrame_When_CalledWithFormat()
        {
            using var imageFrame = new ImageFrame(ImageFormat.Sbgra, 640, 480);
            Assert.AreEqual(imageFrame.Format, ImageFormat.Sbgra);
            Assert.AreEqual(imageFrame.Width, 640);
            Assert.AreEqual(imageFrame.Height, 480);
            Assert.AreEqual(imageFrame.ChannelSize, 1);
            Assert.AreEqual(imageFrame.NumberOfChannels, 4);
            Assert.AreEqual(imageFrame.ByteDepth, 1);
            Assert.AreEqual(imageFrame.WidthStep, 640 * 4);
            Assert.AreEqual(imageFrame.PixelDataSize, 640 * 480 * 4);
            Assert.AreEqual(imageFrame.PixelDataSizeStoredContiguously, 640 * 480 * 4);
            Assert.False(imageFrame.IsEmpty);
            Assert.True(imageFrame.IsContiguous);
            Assert.True(imageFrame.IsAligned(16));
            unsafe
            {
                Assert.True(imageFrame.MutablePixelData != null);
            }
        }

        [Test]
        public void Ctor_ShouldInstantiateImageFrame_When_CalledWithFormatAndAlignmentBoundary()
        {
            using var imageFrame = new ImageFrame(ImageFormat.Gray8, 100, 100, 8);
            Assert.AreEqual(imageFrame.Width, 100);
            Assert.AreEqual(imageFrame.NumberOfChannels, 1);
            Assert.AreEqual(imageFrame.WidthStep, 104);
        }

        [Test]
        unsafe public void Ctor_ShouldInstantiateImageFrame_When_CalledWithPixelData()
        {
            byte[] srcBytes = new byte[] {
                0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15,
                16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31,
            };
            byte[] dupBytes = srcBytes.ToArray();

            fixed (byte* pixelData = dupBytes)
            {
                using var imageFrame = new ImageFrame(ImageFormat.Sbgra, 4, 2, 16, pixelData);
                Assert.AreEqual(imageFrame.Width, 4);
                Assert.AreEqual(imageFrame.Height, 2);
                Assert.False(imageFrame.IsEmpty);

                var bytes = imageFrame.CopyToByteBuffer(32);
                Assert.IsEmpty(bytes.Where((x, i) => x != srcBytes[i]));
            }
        }

        [Test, SignalAbort]
        public void Ctor_ShouldThrowMediapipeException_When_CalledWithInvalidArgument()
        {
#pragma warning disable IDE0058
            Assert.Throws<MediapipeException>(() => { new ImageFrame(ImageFormat.Sbgra, 640, 480, 0); });
#pragma warning restore IDE0058
        }
        #endregion

        #region IsDisposed
        [Test]
        public void IsDisposed_ShouldReturnFalse_When_NotDisposedYet()
        {
            using var imageFrame = new ImageFrame();
            Assert.False(imageFrame.IsDisposed);
        }

        [Test]
        public void IsDisposed_ShouldReturnTrue_When_AlreadyDisposed()
        {
            var imageFrame = new ImageFrame();
            imageFrame.Dispose();

            Assert.True(imageFrame.IsDisposed);
        }
        #endregion

        #region SetToZero
        [Test]
        public void SetToZero_ShouldSetZeroToAllBytes()
        {
            using var imageFrame = new ImageFrame(ImageFormat.Gray8, 10, 10);
            var origBytes = imageFrame.CopyToByteBuffer(100);

            imageFrame.SetToZero();
            var bytes = imageFrame.CopyToByteBuffer(100);
            Assert.True(bytes.All((x) => x == 0));
        }
        #endregion

        #region SetAlignmentPaddingAreas
        [Test]
        public void SetAlignmentPaddingAreas_ShouldNotThrow()
        {
            using var imageFrame = new ImageFrame(ImageFormat.Gray8, 10, 10, 16);
            Assert.DoesNotThrow(() => { imageFrame.SetAlignmentPaddingAreas(); });
        }
        #endregion

        #region CopyToBuffer
        [Test]
        public void CopyToByteBuffer_ShouldReturnByteArray_When_BufferSizeIsLargeEnough()
        {
            using var imageFrame = new ImageFrame(ImageFormat.Gray8, 10, 10);
            var normalBuffer = imageFrame.CopyToByteBuffer(100);
            var largeBuffer = imageFrame.CopyToByteBuffer(120);

            Assert.IsEmpty(normalBuffer.Where((x, i) => x != largeBuffer[i]));
        }

        [Test, SignalAbort]
        public void CopyToByteBuffer_ShouldThrowException_When_BufferSizeIsTooSmall()
        {
            using var imageFrame = new ImageFrame(ImageFormat.Gray8, 10, 10);
#pragma warning disable IDE0058
            Assert.Throws<MediapipeException>(() => { imageFrame.CopyToByteBuffer(99); });
#pragma warning restore IDE0058
        }

        [Test]
        public void CopyToUshortBuffer_ShouldReturnUshortArray_When_BufferSizeIsLargeEnough()
        {
            using var imageFrame = new ImageFrame(ImageFormat.Gray16, 10, 10);
            var normalBuffer = imageFrame.CopyToUshortBuffer(100);
            var largeBuffer = imageFrame.CopyToUshortBuffer(120);

            Assert.IsEmpty(normalBuffer.Where((x, i) => x != largeBuffer[i]));
        }

        [Test, SignalAbort]
        public void CopyToUshortBuffer_ShouldThrowException_When_BufferSizeIsTooSmall()
        {
            using var imageFrame = new ImageFrame(ImageFormat.Gray16, 10, 10);
#pragma warning disable IDE0058
            Assert.Throws<MediapipeException>(() => { imageFrame.CopyToUshortBuffer(99); });
#pragma warning restore IDE0058
        }

        [Test]
        public void CopyToFloatBuffer_ShouldReturnFloatArray_When_BufferSizeIsLargeEnough()
        {
            using var imageFrame = new ImageFrame(ImageFormat.Vec32f1, 10, 10);
            var normalBuffer = imageFrame.CopyToFloatBuffer(100);
            var largeBuffer = imageFrame.CopyToFloatBuffer(120);

            Assert.IsEmpty(normalBuffer.Where((x, i) => Math.Abs(x - largeBuffer[i]) > 1e-9));
        }

        [Test, SignalAbort]
        public void CopyToFloatBuffer_ShouldThrowException_When_BufferSizeIsTooSmall()
        {
            using var imageFrame = new ImageFrame(ImageFormat.Vec32f1, 10, 10);
#pragma warning disable IDE0058
            Assert.Throws<MediapipeException>(() => { imageFrame.CopyToFloatBuffer(99); });
#pragma warning restore IDE0058
        }
        #endregion
    }
}
