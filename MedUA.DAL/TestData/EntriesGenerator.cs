using System;
using System.Collections.Generic;

namespace MedUA.DAL.UserBuilder
{
    class EntriesGenerator
    {
        public Dictionary<string, Entry[]> DoctorPosition()
        {
            return new Dictionary<string, Entry[]>()
                   {
                       {
                           "Окуліст",
                           new[]
                           {
                               new Entry()
                               {
                                   Difficulties = "Гідродинамічний чинник проявляється вірогідним підвищенням тиску в оці.",
                                   Complains = "Чітко бачить лише близько розташовані предмети.",
                                   Examination = "Підвищена заломлювальна сила оптичних середовищ ока.",
                                   Recomendations = "Окуляри до 2 діоптрій",
                                   Diagnosis = "Короткозорість 2+",
                                   TimeStamp = GenerateDateTime()
                               },
                               new Entry()
                               {
                                   Difficulties = "Неправильний астигматизм ока.",
                                   Complains = "Не чітко бачить лівим оком.",
                                   Examination = "На сітківці утворюється спотворене зображення..",
                                   Recomendations = "Окуляри з циліндричними скельцями",
                                   Diagnosis = "Астигматизм ока",
                                   TimeStamp = GenerateDateTime()
                               }
                           }
                       },
                       {
                           "Терапевт",
                           new[]
                           {
                               new Entry()
                               {
                                   Complains =
                                       "Постійна втома, яка супроводжується дискомфортом і стресами, різке схуднення",
                                   Examination =
                                       "Підвищений артеріальний тиск, гіпертензія, які відносяться до класу серцево-судинної системи.",
                                   Recomendations = "Валемідін 2 р/день протягом неділі",
                                   Researches =
                                       new List<Research>() { new Research() { Name = "Аналіз крові" } },
                                   TimeStamp = GenerateDateTime()
                               },
                               new Entry()
                               {
                                   Complains = "Невгамовна справа, порушення харчової поведінки.",
                                   Examination = "Рівень глюкози в крові підвищений",
                                   Recomendations = "Піронін 1 р/день",
                                   Diagnosis = "Цукровий діабет",
                                   TimeStamp = GenerateDateTime()
                               }
                           }
                       },
                       {
                           "Невролог",
                           new []
                           {
                               new Entry()
                               {
                                   Difficulties = "Важка черепно-мозкова травма, енцефаліт, гостре порушення мозкового кровообігу",
                                   Complains = "Короткочасна втрата свідомості",
                                   Examination = "Транзиторне порушення мозкового кровообігу. Різкий психоемоційний подразник, перегрівання, падіння артеріального тиску",
                                   Recomendations = " Пацієнт потребує ретельного догляду",
                                   QuestionDiagnosis = "Захворювання серця або ортостатична гіпотензія",
                                   TimeStamp = GenerateDateTime()
                               },
                               new Entry()
                               {
                                   Complains = "Головний біль",
                                   Examination = "Подразнення оболонок головного мозку в результаті запалення",
                                   Recomendations = "Tidin NEXAL протягом неділі",
                                   Researches = new List<Research>() { new Research() { Name = "Остеокальцин" }, new Research() { Name = "Пероксидаза щитоподібної залози, антитіла (ATПO)" }} ,
                                   TimeStamp = GenerateDateTime()
                               }
                           }
                       },
                       {
                           "Оториноларинголог",
                           new []
                           {
                               new Entry()
                               {
                                   Complains = "Шум у вусі, відчуття закладеного вуха",
                                   Examination = "Вражена слухова труба",
                                   Recomendations = " Отіпакс, для заспокоєння больових відчуттів; зігріваючі компреси на вуха",
                                   QuestionDiagnosis = "Гострий середній отит",
                                   Researches = new List<Research>() { new Research() { Name = "Прогестерон" }, new Research() { Name = "Пероксидаза щитоподібної залози, антитіла (ATПO)" } },
                                   TimeStamp = GenerateDateTime()
                               },
                               new Entry()
                               {
                                   Complains = "Головний біль",
                                   Examination = "Сухий кашель в програмі трахеїту, підйом температури до 37-38.",
                                   Recomendations = "Спрей Каметон або Биопарокс",
                                   TimeStamp = GenerateDateTime()
                               }
                           }
                       }
                   };
        }

        private DateTime GenerateDateTime()
        {
            var random = new Random();
            var year = random.Next(2010, 2017);
            var month = random.Next(1, 12);
            var day = random.Next(1, 28);
            var hour = random.Next(9, 19);
            var minute = random.Next(60);
            var second = random.Next(60);
            return new DateTime(year, month, day, hour, minute, second);

        }
    }
}
