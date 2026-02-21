using UnityEngine;

/// <summary>
/// Статический класс для форматирования больших чисел (1к, 1мил, 1млр).
/// </summary>
public static class NumberFormatter
{
    // Массив суффиксов для каждого порядка (10^3, 10^6, 10^9...)
    private static readonly string[] Suffixes = { "", "к", "мил", "млр", "трлн", "квд" };

    /// <summary>
    /// Преобразует число в строку с сокращением.
    /// Примеры: 1500 -> "1.5к", 1000000 -> "1мил", 2500000000 -> "2.5млр"
    /// </summary>
    /// <param name="number">Число для форматирования</param>
    /// <returns>Отформатированная строка</returns>
    public static string Format(double number)
    {
        // Если число 0, сразу возвращаем "0"
        if (number == 0) return "0";

        // Работаем с абсолютным значением, чтобы корректно считать порядок
        double absNumber = Mathf.Abs((float)number);
        int magnitude = 0;

        // Делим на 1000, пока число больше или равно 1000, и есть место в массиве суффиксов
        while (absNumber >= 1000f && magnitude < Suffixes.Length - 1)
        {
            absNumber /= 1000f;
            magnitude++;
        }

        // Форматируем число с 1 знаком после запятой (например, 1.5)
        string formattedNumber = absNumber.ToString("F1");

        // Убираем ".0" в конце, если дробной части нет (чтобы было "1к", а не "1.0к")
        if (formattedNumber.EndsWith(".0"))
        {
            formattedNumber = formattedNumber.Substring(0, formattedNumber.Length - 2);
        }

        // Добавляем суффикс и возвращаем результат
        // Если число было отрицательным, добавляем минус в начало
        return (number < 0 ? "-" : "") + formattedNumber + Suffixes[magnitude];
    }
}