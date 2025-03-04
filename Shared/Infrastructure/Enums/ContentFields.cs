namespace myuzbekistan.Shared;

[Flags]
public enum ContentFields
{
    None = 0,               // Обязательно для "пустого" состояния
    Photos = 1 << 0,        // 1
    Contacts = 1 << 1,  // 2
    AverageCheck = 1 << 2,  // 4
    Conditions = 1 << 3,    // 8
    Description = 1 << 4,   // 16
    Facilities = 1 << 5,    // 32
    WorkingHours = 1 << 6,  // 64
    Location = 1 << 7,      // 128
    //Contacts = 1 << 8,      // 256
    Price = 1 << 9,         // 512
    PriceInDollar = 1 << 10,// 1024
    Languages = 1 << 11,    // 2048
    Address = 1 << 12,      // 4096
    Photo = 1 << 13         // 8192
}
