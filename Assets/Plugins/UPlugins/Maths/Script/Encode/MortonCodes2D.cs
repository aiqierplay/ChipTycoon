﻿/////////////////////////////////////////////////////////////////////////////
//
//  Script   : MortonCodes2D.cs
//  Info     : 2D莫顿码，用于多维数据和一位数据的转换
//  Author   : https://github.com/FrameProjectTeam/Fp.Unity.Utility/blob/main/Assets/Runtime/MortonCodes2D.cs
//  E-mail   : ls9512@vip.qq.com
//
//  Copyright : Aya Game Studio 2022
//
/////////////////////////////////////////////////////////////////////////////
using System.Runtime.CompilerServices;

namespace Aya.Maths
{
    public static class MortonCodes2D
    {
        private const uint EightBitMask = 0xFF;

        // LUT for Morton2D encode X
        private static readonly uint[] EncodeX256 =
        {
            0, 1, 4, 5, 16, 17, 20, 21,
            64, 65, 68, 69, 80, 81, 84, 85,
            256, 257, 260, 261, 272, 273, 276, 277,
            320, 321, 324, 325, 336, 337, 340, 341,
            1024, 1025, 1028, 1029, 1040, 1041, 1044, 1045,
            1088, 1089, 1092, 1093, 1104, 1105, 1108, 1109,
            1280, 1281, 1284, 1285, 1296, 1297, 1300, 1301,
            1344, 1345, 1348, 1349, 1360, 1361, 1364, 1365,
            4096, 4097, 4100, 4101, 4112, 4113, 4116, 4117,
            4160, 4161, 4164, 4165, 4176, 4177, 4180, 4181,
            4352, 4353, 4356, 4357, 4368, 4369, 4372, 4373,
            4416, 4417, 4420, 4421, 4432, 4433, 4436, 4437,
            5120, 5121, 5124, 5125, 5136, 5137, 5140, 5141,
            5184, 5185, 5188, 5189, 5200, 5201, 5204, 5205,
            5376, 5377, 5380, 5381, 5392, 5393, 5396, 5397,
            5440, 5441, 5444, 5445, 5456, 5457, 5460, 5461,
            16384, 16385, 16388, 16389, 16400, 16401, 16404, 16405,
            16448, 16449, 16452, 16453, 16464, 16465, 16468, 16469,
            16640, 16641, 16644, 16645, 16656, 16657, 16660, 16661,
            16704, 16705, 16708, 16709, 16720, 16721, 16724, 16725,
            17408, 17409, 17412, 17413, 17424, 17425, 17428, 17429,
            17472, 17473, 17476, 17477, 17488, 17489, 17492, 17493,
            17664, 17665, 17668, 17669, 17680, 17681, 17684, 17685,
            17728, 17729, 17732, 17733, 17744, 17745, 17748, 17749,
            20480, 20481, 20484, 20485, 20496, 20497, 20500, 20501,
            20544, 20545, 20548, 20549, 20560, 20561, 20564, 20565,
            20736, 20737, 20740, 20741, 20752, 20753, 20756, 20757,
            20800, 20801, 20804, 20805, 20816, 20817, 20820, 20821,
            21504, 21505, 21508, 21509, 21520, 21521, 21524, 21525,
            21568, 21569, 21572, 21573, 21584, 21585, 21588, 21589,
            21760, 21761, 21764, 21765, 21776, 21777, 21780, 21781,
            21824, 21825, 21828, 21829, 21840, 21841, 21844, 21845
        };

        // LUT for Morton2D encode Y
        private static readonly uint[] EncodeY256 =
        {
            0, 2, 8, 10, 32, 34, 40, 42,
            128, 130, 136, 138, 160, 162, 168, 170,
            512, 514, 520, 522, 544, 546, 552, 554,
            640, 642, 648, 650, 672, 674, 680, 682,
            2048, 2050, 2056, 2058, 2080, 2082, 2088, 2090,
            2176, 2178, 2184, 2186, 2208, 2210, 2216, 2218,
            2560, 2562, 2568, 2570, 2592, 2594, 2600, 2602,
            2688, 2690, 2696, 2698, 2720, 2722, 2728, 2730,
            8192, 8194, 8200, 8202, 8224, 8226, 8232, 8234,
            8320, 8322, 8328, 8330, 8352, 8354, 8360, 8362,
            8704, 8706, 8712, 8714, 8736, 8738, 8744, 8746,
            8832, 8834, 8840, 8842, 8864, 8866, 8872, 8874,
            10240, 10242, 10248, 10250, 10272, 10274, 10280, 10282,
            10368, 10370, 10376, 10378, 10400, 10402, 10408, 10410,
            10752, 10754, 10760, 10762, 10784, 10786, 10792, 10794,
            10880, 10882, 10888, 10890, 10912, 10914, 10920, 10922,
            32768, 32770, 32776, 32778, 32800, 32802, 32808, 32810,
            32896, 32898, 32904, 32906, 32928, 32930, 32936, 32938,
            33280, 33282, 33288, 33290, 33312, 33314, 33320, 33322,
            33408, 33410, 33416, 33418, 33440, 33442, 33448, 33450,
            34816, 34818, 34824, 34826, 34848, 34850, 34856, 34858,
            34944, 34946, 34952, 34954, 34976, 34978, 34984, 34986,
            35328, 35330, 35336, 35338, 35360, 35362, 35368, 35370,
            35456, 35458, 35464, 35466, 35488, 35490, 35496, 35498,
            40960, 40962, 40968, 40970, 40992, 40994, 41000, 41002,
            41088, 41090, 41096, 41098, 41120, 41122, 41128, 41130,
            41472, 41474, 41480, 41482, 41504, 41506, 41512, 41514,
            41600, 41602, 41608, 41610, 41632, 41634, 41640, 41642,
            43008, 43010, 43016, 43018, 43040, 43042, 43048, 43050,
            43136, 43138, 43144, 43146, 43168, 43170, 43176, 43178,
            43520, 43522, 43528, 43530, 43552, 43554, 43560, 43562,
            43648, 43650, 43656, 43658, 43680, 43682, 43688, 43690
        };

        // LUT for Morton2D decode X
        private static readonly ushort[] DecodeX256 =
        {
            0, 1, 0, 1, 2, 3, 2, 3, 0, 1, 0, 1, 2, 3, 2, 3,
            4, 5, 4, 5, 6, 7, 6, 7, 4, 5, 4, 5, 6, 7, 6, 7,
            0, 1, 0, 1, 2, 3, 2, 3, 0, 1, 0, 1, 2, 3, 2, 3,
            4, 5, 4, 5, 6, 7, 6, 7, 4, 5, 4, 5, 6, 7, 6, 7,
            8, 9, 8, 9, 10, 11, 10, 11, 8, 9, 8, 9, 10, 11, 10, 11,
            12, 13, 12, 13, 14, 15, 14, 15, 12, 13, 12, 13, 14, 15, 14, 15,
            8, 9, 8, 9, 10, 11, 10, 11, 8, 9, 8, 9, 10, 11, 10, 11,
            12, 13, 12, 13, 14, 15, 14, 15, 12, 13, 12, 13, 14, 15, 14, 15,
            0, 1, 0, 1, 2, 3, 2, 3, 0, 1, 0, 1, 2, 3, 2, 3,
            4, 5, 4, 5, 6, 7, 6, 7, 4, 5, 4, 5, 6, 7, 6, 7,
            0, 1, 0, 1, 2, 3, 2, 3, 0, 1, 0, 1, 2, 3, 2, 3,
            4, 5, 4, 5, 6, 7, 6, 7, 4, 5, 4, 5, 6, 7, 6, 7,
            8, 9, 8, 9, 10, 11, 10, 11, 8, 9, 8, 9, 10, 11, 10, 11,
            12, 13, 12, 13, 14, 15, 14, 15, 12, 13, 12, 13, 14, 15, 14, 15,
            8, 9, 8, 9, 10, 11, 10, 11, 8, 9, 8, 9, 10, 11, 10, 11,
            12, 13, 12, 13, 14, 15, 14, 15, 12, 13, 12, 13, 14, 15, 14, 15
        };

        // LUT for Morton2D decode Y
        private static readonly ushort[] DecodeY256 =
        {
            0, 0, 1, 1, 0, 0, 1, 1, 2, 2, 3, 3, 2, 2, 3, 3,
            0, 0, 1, 1, 0, 0, 1, 1, 2, 2, 3, 3, 2, 2, 3, 3,
            4, 4, 5, 5, 4, 4, 5, 5, 6, 6, 7, 7, 6, 6, 7, 7,
            4, 4, 5, 5, 4, 4, 5, 5, 6, 6, 7, 7, 6, 6, 7, 7,
            0, 0, 1, 1, 0, 0, 1, 1, 2, 2, 3, 3, 2, 2, 3, 3,
            0, 0, 1, 1, 0, 0, 1, 1, 2, 2, 3, 3, 2, 2, 3, 3,
            4, 4, 5, 5, 4, 4, 5, 5, 6, 6, 7, 7, 6, 6, 7, 7,
            4, 4, 5, 5, 4, 4, 5, 5, 6, 6, 7, 7, 6, 6, 7, 7,
            8, 8, 9, 9, 8, 8, 9, 9, 10, 10, 11, 11, 10, 10, 11, 11,
            8, 8, 9, 9, 8, 8, 9, 9, 10, 10, 11, 11, 10, 10, 11, 11,
            12, 12, 13, 13, 12, 12, 13, 13, 14, 14, 15, 15, 14, 14, 15, 15,
            12, 12, 13, 13, 12, 12, 13, 13, 14, 14, 15, 15, 14, 14, 15, 15,
            8, 8, 9, 9, 8, 8, 9, 9, 10, 10, 11, 11, 10, 10, 11, 11,
            8, 8, 9, 9, 8, 8, 9, 9, 10, 10, 11, 11, 10, 10, 11, 11,
            12, 12, 13, 13, 12, 12, 13, 13, 14, 14, 15, 15, 14, 14, 15, 15,
            12, 12, 13, 13, 12, 12, 13, 13, 14, 14, 15, 15, 14, 14, 15, 15
        };

        private static readonly uint[] _magicBit2dMask32 = { 0x0000FFFF, 0x00FF00FF, 0x0F0F0F0F, 0x33333333, 0x55555555 };

        public static uint EncodeSLut(ushort x, ushort y)
        {
            uint answer = 0;

            answer = (answer << 16) | EncodeY256[(y >> 24) & EightBitMask] | EncodeX256[(x >> 24) & EightBitMask];
            answer = (answer << 16) | EncodeY256[(y >> 16) & EightBitMask] | EncodeX256[(x >> 16) & EightBitMask];
            answer = (answer << 16) | EncodeY256[(y >> 8) & EightBitMask] | EncodeX256[(x >> 8) & EightBitMask];
            answer = (answer << 16) | EncodeY256[y & EightBitMask] | EncodeX256[x & EightBitMask];

            return answer;
        }

        public static void DecodeSLut(uint morton, out ushort x, out ushort y)
        {
            x = 0;
            y = 0;

            x |= DecodeX256[morton & EightBitMask];
            y |= DecodeY256[morton & EightBitMask];
            x |= (ushort)(DecodeX256[(morton >> 8) & EightBitMask] << 4);
            y |= (ushort)(DecodeY256[(morton >> 8) & EightBitMask] << 4);
            x |= (ushort)(DecodeX256[(morton >> 16) & EightBitMask] << 8);
            y |= (ushort)(DecodeY256[(morton >> 16) & EightBitMask] << 8);
            x |= (ushort)(DecodeX256[(morton >> 24) & EightBitMask] << 12);
            y |= (ushort)(DecodeY256[(morton >> 24) & EightBitMask] << 12);
        }

        public static void DecodeLut(uint morton, out ushort x, out ushort y)
        {
            x = 0;
            y = 0;

            x |= DecodeX256[morton & EightBitMask];
            y |= DecodeX256[(morton >> 1) & EightBitMask];
            x |= (ushort)(DecodeX256[(morton >> 8) & EightBitMask] << 4);
            y |= (ushort)(DecodeX256[(morton >> 9) & EightBitMask] << 4);
            x |= (ushort)(DecodeX256[(morton >> 16) & EightBitMask] << 8);
            y |= (ushort)(DecodeX256[(morton >> 17) & EightBitMask] << 8);
            x |= (ushort)(DecodeX256[(morton >> 24) & EightBitMask] << 12);
            y |= (ushort)(DecodeX256[(morton >> 25) & EightBitMask] << 12);
        }

        public static uint EncodeLut(ushort x, ushort y)
        {
            uint answer = 0;

            answer = (answer << 16) | (EncodeX256[(y >> 24) & EightBitMask] << 1) | EncodeX256[(x >> 24) & EightBitMask];
            answer = (answer << 16) | (EncodeX256[(y >> 16) & EightBitMask] << 1) | EncodeX256[(x >> 16) & EightBitMask];
            answer = (answer << 16) | (EncodeX256[(y >> 8) & EightBitMask] << 1) | EncodeX256[(x >> 8) & EightBitMask];
            answer = (answer << 16) | (EncodeX256[y & EightBitMask] << 1) | EncodeX256[x & EightBitMask];

            return answer;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Encode(ushort x, ushort y)
        {
            return MortonSplitBy2Bits(x) | (MortonSplitBy2Bits(y) << 1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Decode(uint morton, out ushort x, out ushort y)
        {
            x = MortonGetSecondBits(morton);
            y = MortonGetSecondBits(morton >> 1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint MortonSplitBy2Bits(ushort v)
        {
            uint x = v;
            x = (x | (x << 16)) & _magicBit2dMask32[0];
            x = (x | (x << 8)) & _magicBit2dMask32[1];
            x = (x | (x << 4)) & _magicBit2dMask32[2];
            x = (x | (x << 2)) & _magicBit2dMask32[3];
            x = (x | (x << 1)) & _magicBit2dMask32[4];
            return x;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ushort MortonGetSecondBits(uint morton)
        {
            uint x = morton & _magicBit2dMask32[4];
            x = (x ^ (x >> 1)) & _magicBit2dMask32[3];
            x = (x ^ (x >> 2)) & _magicBit2dMask32[2];
            x = (x ^ (x >> 4)) & _magicBit2dMask32[1];
            x = (x ^ (x >> 8)) & _magicBit2dMask32[0];
            return (ushort)x;
        }
    }
}
