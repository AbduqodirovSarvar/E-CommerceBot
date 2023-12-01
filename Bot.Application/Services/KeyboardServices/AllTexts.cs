using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Application.Services.KeyboardServices
{
    internal static class AllTexts
    {
        public static string[,] MainMenuButtons { get; } = new string[,]
        {
            //0
            { "Buyurtma qilish", "Aloqa", "Ma'lumot", "Fikr bildirish", "Sozlamalar" },
            //1
            { "Order", "Contact", "Info", "Feedback", "Settings" },
            //2
            { "Заказ", "Контакты", "Информация", "Обратная связь", "Настройки" }
        };

        public static string[,] SettingsMenuButtons { get; } = new string[,]
        {
            //0
            { "Ism-Familiyani o'zgartirish", "Telefon raqamni o'zgartish", "Tilni o'zgartirish", "Orqaga" },
            //1
            { "Change name-surname", "Change phone number", "Change language", "Back" },
            //2
            { "Изменить имя-фамилию", "Изменить номер телефона", "Изменить язык", "Назад" }
        };

        public static string[,] ShareContactButtons { get; } = new string[,]
        {
            //0
            { "Kantaktni ulashish", "Share a contact", "Поделиться контактом" }
        };

        public static string[,] OrderMenuButtons { get; } = new string[,]
        {
            //0
            { "Yetkazib berish", "Olib Ketish", "Savatcha", "Orqaga" },
            //1
            { "Delivery", "Take away", "Shopping cart", "Back" },
            //2
            { "Доставка", "Еда на вынос", "Корзина", "Назад" }
        };

        public static string[,] Messages { get; } = new string[,]
        {
            //0
            {"Assalomu aleykum, Botimizga xush kelibsiz", "Hello, welcome to our site", "Здравствуйте, добро пожаловать на наш сайт" },
            //1
            {"Ism-Familyangizni kiriting: ", "Enter your name and surname:",  "Введите свое имя и фамилию:" },
            //2
            {"Tilni tanlang: ", "Choose a language: ", "Выберите язык:" },
            //3
            {"Kantakingizni pastdagi tugma orqali yuboring:", "Send your message using the button below:", "Отправьте свое сообщение, используя кнопку ниже:" },
            //4
            {"Kerakli bo'limni tanlang: ", "Select the desired section:", "Выберите нужный раздел:" },
            //5
            {"E'tiboringiz uchun raxmat!", "Thank you for your attention!", "Спасибо за внимание!" },
            //6
            {"Murojatingizni yuboring: ", "Send your request to:", "Отправьте свой запрос по адресу:" }
        };
    }
}
