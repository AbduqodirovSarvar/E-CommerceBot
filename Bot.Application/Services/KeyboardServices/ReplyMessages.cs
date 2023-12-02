using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Application.Services.KeyboardServices
{
    internal static class ReplyMessages
    {
        public static readonly string[] askRegister = new[] { "Bu buyruqdan foydalanish uchun ro'yhatdan o'ting!", "Register to use this command!", "Зарегистрируйтесь, чтобы использовать эту команду!" };
        public static readonly string[] askChoosingLanguage = new[] { "Tilni tanlang: ", "Choose a language: ", "Выберите язык:" };
        public static readonly string[] afterRegistered = new[] { "Tabriklaymiz!\nSiz ro'yhatdan muvaffaqiyatli o'tdingiz!\nKerakli bo'limni tanlang!", "Congratulations!\nYou have successfully registered!\nChoose the required section!", "Поздравляем!\nВы успешно зарегистрировались!\nВыберите нужный раздел!" };
        public static readonly string[] askFullName = new[] { "Familiyangizni kiriting:", "Enter your last name:", "Введите свою фамилию:" };
        public static readonly string[] askContact = new[] { "Kantaktingizni yuboring:", "Send your contact:", "Отправьте свой контакт:" };
        public static readonly string[] chooseCommand = new[] { "Kerakli bo'limni tanlang:", "Select the desired section:", "Выберите нужный раздел:" };
        public static readonly string[] askFeedback = new[] { "Taklif va shikoyatlaringizni yozing.\nBiz ularni albatta ko'rib chiqamiz!", "Write your suggestions and complaints.\nWe will definitely consider them!", "Пишите свои предложения и жалобы.\nМы обязательно их рассмотрим!" };
        public static readonly string[] unKnownCommand = new[] { "Siz belgilanmagan buyruqni yubordingiz!", "You sent an unsigned command!", "Вы отправили неподписанную команду!" };
        public static readonly string[] feedbackAnswer = new[] { "Befarq bo'lmaganingiz uchun raxmat!", "Thank you for not being indifferent!", "Спасибо, что не остались равнодушными!" };
        public static readonly string[] chooseFilial = new[] { "Filialni tanglang:", "Choose Filial:", "Выберите филиал:" };
        public static readonly string[] helpMessages = new[] { "Assalomu aleykum\nBotimizdan foydalanish uchun /start buyrug'ini bosing!", "Hello\nClick /start to use our bot!", "Здравствуйте!\nНажмите /начать, чтобы использовать нашего бота!" };

        public static readonly string[,] MainMenuButtons = new string[,]
        {
            //0
            { "Buyurtma qilish", "Aloqa", "Ma'lumot", "Fikr bildirish", "Sozlamalar" },
            //1
            { "Order", "Contact", "Info", "Feedback", "Settings" },
            //2
            { "Заказ", "Контакты", "Информация", "Обратная связь", "Настройки" }
        };

        public static readonly string[,] SettingsMenuButtons = new string[,]
        {
            //0
            { "Ism-Familiyani o'zgartirish", "Telefon raqamni o'zgartish", "Tilni o'zgartirish", "Orqaga" },
            //1
            { "Change name-surname", "Change phone number", "Change language", "Back" },
            //2
            { "Изменить имя-фамилию", "Изменить номер телефона", "Изменить язык", "Назад" }
        };

        public static readonly string[] ShareContactButtons = new string[]
        {
            "Kantaktni ulashish", "Share a contact", "Поделиться контактом"
        };

        public static readonly string[,] OrderMenuButtons = new string[,]
        {
            //0
            { "Yetkazib berish", "Olib Ketish", "Savatcha", "Orqaga" },
            //1
            { "Delivery", "Take away", "Shopping cart", "Back" },
            //2
            { "Доставка", "Еда на вынос", "Корзина", "Назад" }
        };
    }
}
