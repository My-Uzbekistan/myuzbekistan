using System;

namespace myuzbekistan.Shared
{
    public static class Bitwise
    {
        // Проверяет, установлен ли K-й бит
        public static bool IsSetKthBit(int n, int k)
        {
            return ((1 << k) & n) != 0;
        }

        // Устанавливает K-й бит
        public static int SetKthBit(int n, int k)
        {
            return n | (1 << k);
        }

        // Сбрасывает K-й бит
        public static int UnSetKthBit(int n, int k)
        {
            return n & ~(1 << k);
        }

        // Проверяет, установлен ли флаг ContentFields
        public static bool Is(int n, ContentFields contentFields)
        {
            return ((ContentFields)n & contentFields) == contentFields;
        }

        // Устанавливает флаг ContentFields
        public static int Set(int n, ContentFields contentFields)
        {
            return n | (int)contentFields;
        }

        // Убирает флаг ContentFields
        public static int UnSet(int n, ContentFields contentFields)
        {
            return n & ~(int)contentFields;
        }
    }
}
