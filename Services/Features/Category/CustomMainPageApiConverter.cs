using myuzbekistan.Shared;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections;
namespace myuzbekistan.Services;

public class CustomMainPageApiConverter : JsonConverter<MainPageApi>
{
    public override MainPageApi? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Используем стандартную десериализацию
        return JsonSerializer.Deserialize<MainPageApi>(ref reader, options);
    }

    public override void Write(Utf8JsonWriter writer, MainPageApi value, JsonSerializerOptions options)
    {
        // Получаем свойства объекта
        var properties = value.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

        writer.WriteStartObject();

        foreach (var property in properties)
        {
            var propValue = property.GetValue(value);
            if (propValue == null) continue;  // Пропускаем null

            if (propValue is IEnumerable enumerable && !(propValue is string))
            {
                // Проверяем, пуст ли массив
                var hasElements = enumerable.GetEnumerator().MoveNext();
                if (!hasElements) continue;  // Пропускаем пустые коллекции
            }

            // Преобразуем имя свойства в camelCase
            var namingPolicyValue = options.PropertyNamingPolicy?.ConvertName(property.Name) ?? property.Name;

            // Сериализуем оставшееся свойство
            writer.WritePropertyName(namingPolicyValue);
            JsonSerializer.Serialize(writer, propValue, options);
        }

        writer.WriteEndObject();
    }
}
