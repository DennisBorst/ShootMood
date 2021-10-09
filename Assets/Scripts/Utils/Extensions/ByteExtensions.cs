﻿using System.Linq;

namespace Oasez.Extensions.Byte {

    public static class ByteExtensions {

        /// <summary>
        /// Combines byte arrays together
        /// </summary>
        /// <param name="arrays"></param>
        /// <returns></returns>
        public static byte[] Combine(params byte[][] arrays) {
            byte[] rv = new byte[arrays.Sum(a => a.Length)];
            int offset = 0;
            foreach (byte[] array in arrays) {
                System.Buffer.BlockCopy(array, 0, rv, offset, array.Length);
                offset += array.Length;
            }
            return rv;
        }

    }

}