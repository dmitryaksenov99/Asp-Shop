using AspShop.Models.Enums;
using AspShop.Models.Items;
using System;

namespace AspShop.Helpers.Generators
{
    public static class GoodGenerator
    {
        static Random rnd = new Random();
        public static Item Laptop()
        {
            var brand = new string[3] { "Acer", "Dell", "Asus" };
            var model = new string[9] { "RS", "SK", "DX", "ZW", "PK", "EU", "ND", "XW", "GA" };
            var img = new string[12] { "DKiSuxa", "58MJtHh", "XtbKBj5", "GIXmCc8", "8qw5sJJ", "WoxCodO", "pqfkROA", "c0byjrH", "uQrhEIs", "Kv2lqIM", "8nlSObx", "Ov700dN" };
            var cpu = new byte[3] { 5, 7, 9 };
            var ram = new byte[3] { 8, 16, 32 };
            var storage = new byte[3] { 1, 2, 4 };
            var videocard = new short[3] { 1060, 1070, 1080 };

            var rbrand = brand[rnd.Next(brand.Length)];
            var rmodel = model[rnd.Next(model.Length)];

            var item = new Item
            {
                Code = RandomStringGenerator.GenerateItemCode(),
                Name = rbrand + " " + rmodel + rnd.Next(short.MaxValue, int.MaxValue),
                Price = rnd.Next(500, 10001),
                Image = "https://i.imgur.com/" + img[rnd.Next(img.Length)] + ".jpg",
                Brand = rbrand,
                Description =
                @"Ноутбуки " + rbrand + @" серии " + rmodel + @" могут похвастать великолепными мультимедийными возможностями.
                Эксклюзивная аудиотехнология SonicMaster, а также фирменные технологии " + rbrand + @" Splendid и " + rbrand + @" Tru2Life Video, обеспечивают беспрецедентное для мобильных компьютеров качество звучания и отличное изображение.
                Аудиотехнология SonicMaster обеспечивает кристально чистый звук.
                Кроме того, динамики ноутбука подвергаются высокоточной настройке для гарантированно высокого качества звучания.
                Для настройки звучания служит функция AudioWizard, предлагающая выбрать один из пяти вариантов работы аудиосистемы, каждый из которых идеально подходит для определенного типа приложений (музыка, фильмы, игры и т.д.).
                Технология " + rbrand + @" TruLife Video оптимизирует резкость и контрастность видео на уровне отдельных пикселей для более точной передачи оттенков и улучшения качества изображения в целом.
                Короткие электромагнитные волны, соответствующие пурпурно-синему краю спектра, обладают большей энергией, поэтому оказывают более сильный эффект на сетчатку глаза.
                В режиме " + rbrand + @" Eye Care реализована фильтрация этой составляющей видимого спектра для повышения комфорта при чтении и снижения вредного влияния синего света на зрение.
                Ноутбуки " + rbrand + @" серии VivoBook " + rmodel + @" оснащаются эргономичной клавиатурой и высокоточным тачпадом, снабженным защитой от случайных прикосновений.",
                Laptop = new Laptop
                {

                    Cpu = "Intel Core i" + cpu[rnd.Next(cpu.Length)],
                    ScreenDiagonal = rnd.Next(9, 18),
                    RamSize = ram[rnd.Next(ram.Length)],
                    RamType = RamType.DDR3,
                    Os = "Windows 10 Pro",
                    Storage = "SSD " + rnd.Next(128, 513) + "gb HDD " + storage[rnd.Next(storage.Length)] + "tb",
                    AdditionalFatures = "Встроенный микрофон, Встроенные динамики",
                    GraphicsAdapter = "nVidia GeForce GTX" + videocard[rnd.Next(0, 2)],
                    Connectors = rnd.Next(1, 3) + " x USB 2.0 / " + rnd.Next(1, 3) + " x USB 3.1 Gen1 / " + rnd.Next(1, 3) + " x USB 3.1 Gen1 Type - C / HDMI /",
                    Battery = "Съёмная, " + rnd.Next(30, 71) + " Вт*ч",
                    Dimensions = rnd.Next(350, 381) + " x " + rnd.Next(220, 251) + " x " + rnd.Next(15, 26),
                    Weight = rnd.Next(1, 6)
                },
                Category = Category.Laptop.ToString()
            };

            return item;
            /*db.Items.Add(item);
            db.SaveChanges();*/
        }
        public static Item Smartphone()
        {
            var brand = new string[3] { "Samsung", "Xiaomi", "Asus" };
            var model = new string[9] { "RS", "SK", "DX", "ZW", "PK", "EU", "ND", "XW", "GA" };
            var img = new string[7] { "rPGDCDc", "myXMKet", "K3Xh4uv", "WAyOLqK", "M85ScsG", "RXrS25y", "z1QC4MG" };
            var dtype = new DisplayType[3] { DisplayType.IPS, DisplayType.IPS, DisplayType.IPS, };
            var storage = new byte[3] { 1, 2, 4 };

            var rbrand = brand[rnd.Next(brand.Length)];
            var rmodel = model[rnd.Next(model.Length)];
            var rdtype = dtype[rnd.Next(dtype.Length)];
            var rstorage = storage[rnd.Next(storage.Length)];

            var item = new Item
            {
                Code = RandomStringGenerator.GenerateItemCode(),
                Name = rbrand + " " + rmodel + rnd.Next(short.MaxValue, int.MaxValue),
                Price = rnd.Next(500, 10001),
                Image = "https://i.imgur.com/" + img[rnd.Next(img.Length)] + ".jpg",
                Description =
                @"
                Благодаря технологии динамического отображения тона, богатой цветовой гамме и продвинутому контрасту HDR+ цвета на ваших видео такие же, как в жизни. Еще в камере есть возможность записать любое действие в мельчайших деталях со скоростью съемки 960 кадров в секунду и поддержкой HD.
                Обратная беспроводная зарядка дает вам суперспособность делиться зарядом с другими устройствами и заряжать ваш смартфон еще быстрее.
                Новое поколение Wi-Fi 6 работает еще быстрее, безопасно подключается даже в публичных сетях и при необходимости автоматически переключается на LTE, чтобы обеспечить высокоскоростную передачу данных до 2 Гб/с.
                Иммерсивный экран с высоким разрешением и игровой режим с поддержкой Dolby обеспечивают реалистичное изображение и улучшенное звучание. А благодаря оптимизационному игровому движку Unity смартфон справится даже с самыми ресурсоемкими играми.
                Оптимизированное аппаратное обеспечение с системой охлаждения, улучшенной производительностью на основе искусственного интеллекта и усовершенствованным графическим процессором обеспечивает плавный игровой процесс и защищает высокомощный телефон от перегрева. А благодаря увеличенной работе аккумулятора не нужно волноваться, что смартфон разрядится в разгаре эпичного игрового сражения.
                ",
                Brand = rbrand,
                Category = Category.Smartphone.ToString(),
                Smartphone = new Smartphone
                {
                    DisplayDiagonal = rnd.Next(0, 8),
                    DisplayType = rdtype,
                    RamSize = rnd.Next(0, 9),
                    Storage = rstorage + "gb",
                    Battery = "Съёмная, " + rnd.Next(30, 71) + " Вт*ч",
                    Dimensions = rnd.Next(35, 38) + " x " + rnd.Next(22, 25) + " x " + rnd.Next(1, 3),
                    Weight = rnd.Next(100, 300)
                }
            };

            return item;
            /*db.Items.Add(item);
            db.SaveChanges();*/
        }
    }
}