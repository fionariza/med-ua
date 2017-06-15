namespace MedUA.DAL.UserBuilder
{
    using System;
    using System.Collections.Generic;

    using MedUA.DAL.TestData;

    class TestDataGenerator
    {
        public static IEnumerable<string> GetResearches()
        {
            yield return "Пероксидаза щитоподібної залози, антитіла(ATПO)";
            yield return "Тиреоглобулін, антитіла(АТТГ)";
            yield return "Тиреоглобулін(ТГ)";
            yield return "Тиреотропний гормон (ТТГ)";
            yield return "Тироксин вільний";
            yield return "Тироксин загальний";
            yield return "Трийодтиронин вільний";
            yield return "Трийодтиронин загальний";
            yield return "Паратгормон(ПТГ)";
            yield return "Остеокальцин";
            yield return "Вітамін D загальний(D2 + D3)";
            yield return "Маркер кісткової резорбції";
            yield return "Остаза(кількісне визначення)";
            yield return "Маркер формування кісткового матриксу(Total P1NP)";
            yield return "Лютеїнізуючий гормон(ЛГ)";
            yield return "Пролактин";
            yield return "Тестостерон загальний(Т загальний)";
            yield return "Фолікулостимулюючий гормон(ФСГ)";
            yield return "Естрадіол(E2)";
            yield return "Тестостерон вільний(Т вільний)";
            yield return "Антимюллерів гормон(АМГ, MIS)";
            yield return "Дигідротестостерон";
            yield return "Андростендіон";
            yield return "Макропролактин";
            yield return "Естрон(E1)";
            yield return "Мікросоми печінки і нирок 1, антитіла IgG(LKM - 1, кількісне визначення)";
            yield return "ДНК односпіральна, антитіла IgG";
            yield return "ДНК двоспіральна(ANA - Screen), антитіла IgG";
            yield return "Цитозольний антиген типу 1 печінки, антитіла IgG";
            yield return "Хроматин(ANA - Screen), антитіла IgG";
            yield return "Рибосомальний протеїн(ANA - Screen), антитіла IgG";
        }

        public static IEnumerable<Place> GetPlaces()
        {
            yield return new Place() { SettlementName = "КИЇВ", Oblast = "Київська", Region = "Києво-Святошинський" };
            yield return new Place() { SettlementName = "ЗАПОРІЖЖЯ", Oblast = "Запорізька", Region = "Запорізький" };
            yield return new Place() { SettlementName = "КОЗАЧЕ", Oblast = "Одеська", Region = "Біляївський" };
            yield return new Place() { SettlementName = "ЛЬВІВ", Oblast = "Львівська", Region = "Львів" };
            yield return new Place() { SettlementName = "МАСЛОВА", Oblast = "Одеська", Region = "Роздільнянський" };
            yield return new Place() { SettlementName = "ПЕТРІВКА", Oblast = "Одеська", Region = "Савранський" };
            yield return new Place() { SettlementName = "ОДЕСА", Oblast = "Одеська", Region = "Одеса" };
            yield return new Place() { SettlementName = "ДЗИНІЛОР", Oblast = "Одеська", Region = "Лиманський" };
            yield return new Place() { SettlementName = "КІЛІЯ", Oblast = "Одеська", Region = "Лиманський" };
            yield return new Place() { SettlementName = "ЛІСКИ", Oblast = "Одеська", Region = "Савранський" };
            yield return new Place() { SettlementName = "МИКОЛАЇВКА", Oblast = "Вінницька", Region = "Піщанський" };
            yield return new Place() { SettlementName = "КРИВИЙ РІГ", Oblast = "Дніпропетровська", Region = "Криворізький" };
            yield return new Place() { SettlementName = "МИРНЕ", Oblast = "Дніпропетровська", Region = "Криворізький" };
            yield return new Place() { SettlementName = "НОВОМИКОЛАЇВКА", Oblast = "Дніпропетровська", Region = "Дніпровський" };
            yield return new Place() { SettlementName = "НОВОСЕЛІВКА", Oblast = "Дніпропетровська", Region = "Криворізький" };
            yield return new Place() { SettlementName = "ПОМАЗАНИ", Oblast = "Дніпропетровська", Region = "Криворізький" };
            yield return new Place() { SettlementName = "ДНІПРО", Oblast = "Дніпропетровська", Region = "Дніпровський" };
            yield return new Place() { SettlementName = "ПРИМОРСЬКЕ", Oblast = "Дніпропетровська", Region = "Нікопольський" };
            yield return new Place() { SettlementName = "ПРИОЗЕРНЕ", Oblast = "Дніпропетровська", Region = "Нікопольський" };
            yield return new Place() { SettlementName = "ШЕВЧЕНКОВЕ", Oblast = "Дніпропетровська", Region = "Покровський" };
            yield return new Place() { SettlementName = "СТАРІ ТРОЯНИ", Oblast = "Дніпропетровська", Region = "Нікопольський" };
            yield return new Place() { SettlementName = "ТРУДОВЕ", Oblast = "Дніпропетровська", Region = "Покровський" };
            yield return new Place() { SettlementName = "ВАСИЛІВКА", Oblast = "Дніпропетровська ", Region = "Покровський" };
        }
        public static IEnumerable<NamesAndAliases> GetWomenNames()
        {
            yield return new NamesAndAliases("olga", "Ольга", "Гліб", "Віталіївна");
            yield return new NamesAndAliases("inna", "Інна", "Воронник", "Георгієвна");
            yield return new NamesAndAliases("maryna", "Марина", "Печерко", "Ігоревна");
            yield return new NamesAndAliases("anastasya", "Анастасія", "Державенко", "Миколаївна");
            yield return new NamesAndAliases("veronika", "Вероніка", "Драч", "Михайлівна");
            yield return new NamesAndAliases("natalya", "Наталля", "Голуб", "Дмитрівна");
            yield return new NamesAndAliases("iryna", "Ірина", "Мирська", "Василівна");
            yield return new NamesAndAliases("tetyana", "Тетяна", "Любимова", "Остапівна");
            yield return new NamesAndAliases("miroslava", "Віресенко", "Северський", "Дмитрівна");
            yield return new NamesAndAliases("anna", "Анна", "Іваненко", "Захарівна");
        }

        public static IEnumerable<NamesAndAliases> GetMenNames()
        {
            yield return new NamesAndAliases("igor", "Ігор", "Іваненко", "Віталійович");
            yield return new NamesAndAliases("vitaliy", "Віталій", "Некидайло", "Георгійович");
            yield return new NamesAndAliases("dmytriy", "Дмитрій", "Воронник", "Ігоревич");
            yield return new NamesAndAliases("gnat", "Гнат", "Печерко", "Миколайович");
            yield return new NamesAndAliases("micolay", "Миколай", "Державенко", "Михайлович");
            yield return new NamesAndAliases("sviatoslav", "Святослав", "Драч", "Григорович");
            yield return new NamesAndAliases("oleksandr", "Олександр", "Голуб", "Дмитрійович");
            yield return new NamesAndAliases("taras", "Тарас", "Мирський", "Васильович");
            yield return new NamesAndAliases("myroslav", "Мирослав", "Северський", "Остапович");
            yield return new NamesAndAliases("victor", "Віктор", "Віресенко", "Захарович");
        }
    }

}
