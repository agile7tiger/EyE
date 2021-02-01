﻿using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace EyE.Shared.Helpers
{
    public static class ImageHelper
    {
        private static readonly Dictionary<byte[], Func<BinaryReader, ImageInfo>> imageFormatDecoders = 
            new Dictionary<byte[], Func<BinaryReader, ImageInfo>>()
        {
            { new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A }, DecodePng },
            { new byte[] { 0xff, 0xd8 }, DecodeJfif },
        };

        public static bool IsHorizontalImage(Stream stream)
        {
            var imageInfo = GetImageInfo(stream);

            if (imageInfo.Width / imageInfo.Height > 1)
                return true;

            return false;
        }

        private static ImageInfo GetImageInfo(Stream stream)
        {
            using (var reader = new BinaryReader(stream))
            {
                var maxMagicBytesLength = imageFormatDecoders.Keys.OrderByDescending(x => x.Length).First().Length;
                var magicBytes = new byte[maxMagicBytesLength];

                for (var i = 0; i < maxMagicBytesLength; i++)
                {
                    magicBytes[i] = reader.ReadByte();

                    foreach (var pair in imageFormatDecoders)
                    {
                        if (StartsWith(magicBytes, pair.Key))
                        {
                            return pair.Value(reader);
                        }
                    }
                }
            }

            return default;
        }

        private static bool StartsWith(byte[] thisBytes, byte[] thatBytes)
        {
            for (var i = 0; i < thatBytes.Length; i++)
                if (thisBytes[i] != thatBytes[i])
                    return false;

            return true;
        }

        private static ImageInfo DecodeJfif(BinaryReader binaryReader)
        {
            while (binaryReader.ReadByte() == 0xff)
            {
                var marker = binaryReader.ReadByte();
                var chunkLength = ReadLittleEndianInt16(binaryReader);

                if (marker == 0xc0)
                {
                    binaryReader.ReadByte();
                    var height = ReadLittleEndianInt16(binaryReader);
                    var width = ReadLittleEndianInt16(binaryReader);
                    return new ImageInfo(width, height);
                }

                binaryReader.ReadBytes(chunkLength - 2);
            }

            return default;
        }

        private static ImageInfo DecodePng(BinaryReader reader)
        {
            reader.ReadBytes(8);
            var width = ReadLittleEndianInt32(reader);
            var height = ReadLittleEndianInt32(reader);
            return new ImageInfo(height, width);
        }

        private static short ReadLittleEndianInt16(BinaryReader binaryReader)
        {
            var bytes = new byte[sizeof(short)];

            for (int i = 0; i < bytes.Length; i += 1)
                bytes[bytes.Length - 1 - i] = binaryReader.ReadByte();

            return BitConverter.ToInt16(bytes, 0);
        }

        private static int ReadLittleEndianInt32(BinaryReader reader)
        {
            var bytes = new byte[sizeof(int)];

            for (var i = bytes.Length - 1; i >= 0; i--)
                bytes[i] = reader.ReadByte();

            return BitConverter.ToInt32(bytes, 0);
        }

        private class ImageInfo
        {
            public int Height { get; set; }
            public int Width { get; set; }

            public ImageInfo(int height, int width)
            {
                Height = height;
                Width = width;
            }
        }

    }
}