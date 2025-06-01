using myuzbekistan.Shared;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections;
namespace myuzbekistan.Services;

public class TrimNullableConverter<T> : System.Text.Json.Serialization.JsonConverter<T>
{
    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Используем стандартную десериализацию
        return System.Text.Json.JsonSerializer.Deserialize<T>(ref reader, options);
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        if (value == null)
        {
            writer.WriteEndObject();
            return;
        }

        var properties = value.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

        foreach (var property in properties)
        {
            var propValue = property.GetValue(value);

            // Пропускаем null
            if (propValue == null) continue;

            // Пропускаем пустые коллекции (кроме строки)
            if (propValue is IEnumerable enumerable && !(propValue is string))
            {
                var enumerator = enumerable.GetEnumerator();
                if (!enumerator.MoveNext()) continue;
            }

            // Пропускаем числовые значения, равные 0
            if (propValue is int intValue && intValue == 0) continue;
            if (propValue is long longValue && longValue == 0L) continue;
            if (propValue is float floatValue && Math.Abs(floatValue) < float.Epsilon) continue;
            if (propValue is double doubleValue && Math.Abs(doubleValue) < double.Epsilon) continue;
            if (propValue is decimal decimalValue && decimalValue == 0M) continue;

            // Преобразуем имя свойства в camelCase
            var namingPolicyValue = options.PropertyNamingPolicy?.ConvertName(property.Name) ?? property.Name;

            writer.WritePropertyName(namingPolicyValue);
            System.Text.Json.JsonSerializer.Serialize(writer, propValue, options);
        }

        writer.WriteEndObject();
    }
}
