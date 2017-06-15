using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MedUATests
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using CsvHelper;

    using MedUA.Data;
    using MedUA.DAL;
    using MedUA.Models;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    [TestClass]
    public class PerformanceTesting
    {
        private static int count;

        private static string FileName;

        private static CsvWriter csvHelper;

        private int _count = 10;

        private object lockObject = new object();

        private ApplicationDbContext context;

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            count = 10000;
            FileName = $"../../TestResults/Performance Tests {count}.csv";

            csvHelper = new CsvWriter(new StreamWriter(File.Create(FileName)));
            csvHelper.Configuration.HasHeaderRecord = false;
            csvHelper.WriteField("Method");
            csvHelper.WriteField("Count");
            csvHelper.WriteField("Time");
            csvHelper.NextRecord();

        }

        [TestInitialize]
        public void TestInit()
        {
            this.context = new ApplicationDbContext();
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            csvHelper?.Dispose();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            context?.Dispose();
        }

        //[TestMethod]
        //public void GetResearchProvider_PerformanceTesting()
        //{
        //    var method = new Func<bool>(() =>
        //        {
        //            var result = this.CreateSut().GetResearches().ToList().Any();
        //            this.Count();
        //            return result;
        //        });

        //    PerformTesting("Get researches", method);
        //}

        [TestMethod]
        public void GetListEntriesFilterPatientId_PerformanceTesting()
        {
            var id = this.GetLastPatientWithEntries().Id;
            var method = new Func<bool>(() =>
            {
                var result = this.CreateSut().GetListEntries(id).Any();
                this.Count();
                return result;
            });

            PerformTesting("Entries filtered by patient", method);
        }

        [TestMethod]
        public void GetListEntriesFilterPatientIdAndDoctorId_PerformanceTesting()
        {
            var patient = this.GetLastPatientWithEntries();
            var doctorId = patient.Entries.First().Doctor.Id;
            var method = new Func<bool>(() =>
            {
                var result = this.CreateSut().GetListEntries(patient.Id, doctorId).Any();
                this.Count();
                return result;
            });

            PerformTesting("Entries filtered by patient and doctor", method);
        }

        [TestMethod]
        public void FindDoctorByIdAsync_PerformanceTesting()
        {
            var id = this.GetLastDoctor().Id;
            var method = new Func<bool>(
                () =>
                    {
                        var result = CreateSut().FindDoctorByIdAsync(id).GetAwaiter().GetResult();
                        Count();
                        return result != null;
                    });
            PerformTesting("Find doctor by id async", method);
        }

        [TestMethod]
        public void SearchPatientBySNP_PerformanceTesting()
        {
            var doctor = this.GetLastDoctor();
            var patient = doctor.PatientUsers.Last();
            var searchString = $"{patient.Surname} {patient.Name} {patient.Partonimic}";
            var method = new Func<bool>(
                () =>
                {
                    var result = CreateSut().GetPatientsSearch(doctor, new PatientSearchViewModel() { SearchString = searchString });
                    Count();
                    return result != null;
                });
            PerformTesting("Search patient by surname, name and patronimic", method);
        }

        [TestMethod]
        public void SearchPatientSNPAndPlaceOfBirth_PerformanceTesting()
        {
            var doctor = this.GetLastDoctor();
            var patient = doctor.PatientUsers.Last();
            var searchString = $"{patient.Surname} {patient.Name} {patient.Partonimic} {patient.PlaceOfBirth}";
            var method = new Func<bool>(
                () =>
                {
                    var result = CreateSut().GetPatientsSearch(doctor, new PatientSearchViewModel() { SearchString = searchString });
                    Count();
                    return result != null;
                });
            PerformTesting("Search patient by surname, name, patrominic, place of birth", method);
        }

        [TestMethod]
        public void SearchPatientByMedicalCode_PerformanceTesting()
        {
            var doctor = this.GetLastDoctor();
            var patient = doctor.PatientUsers.Last();
            var searchString = $"{patient.Code}";
            var method = new Func<bool>(
                () =>
                {
                    var result = CreateSut().GetPatientsSearch(doctor, new PatientSearchViewModel() { SearchString = searchString });
                    Count();
                    return result != null;
                });
            PerformTesting("Search patient by medical code", method);
        }

        [TestMethod]
        public void GetPatientListViewModelAsync_PerformanceTesting()
        {
            var doctor = this.GetLastDoctor();
            var method = new Func<bool>(
                () =>
                {
                    var result = CreateSut().GetPatientListViewModelAsync(doctor.Id);
                    Count();
                    return result != null;
                });
            PerformTesting("Get patient list async", method);
        }

        [TestMethod]
        public void SaveEntry_PerformanceTesting()
        {
            var lastId = context.Entries.ToList().Last().Id;
            try
            {
                var patient = this.GetLastPatientWithEntries();
                var entry = patient.Entries.ToList().Last();
                Assert.IsNotNull(entry.Doctor);
                var entryModel = EntryHistoryViewModel.Convert(entry);
                var method = new Func<bool>(
                    () =>
                    {
                        var result = CreateSut().SaveEntry(entryModel);
                        Count();
                        return result != null;
                    });
                PerformTesting("Save entry", method);
            }
            finally
            {
                context.Entries.RemoveRange(context.Entries.Where(x => x.Id > lastId));
                context.SaveChanges();
                context.Dispose();
            }
        }


        [TestMethod]
        public void AddPatient_PerformanceTesting()
        {
            var lastId = context.Patients.ToList().Last().Id;
            try
            {
                var doctors = this.GenerateDoctors();
                var method = new Func<int, bool>(
                    (index) =>
                        {
                            var result = CreateSut().AddPatient(doctors[index], lastId).GetAwaiter().GetResult();
                            Count();
                            return result;
                        });
                PerformTesting("Add patient to doctor", method);
                Assert.IsTrue(RemovDoctorsAndCheck(lastId));
            }
            catch
            {
                RemoveDoctors();
                throw;
            }

        }
        private void PerformTesting(string name, Action<int> action)
        {
            try
            {
                Assert.IsTrue(this._count == 0);
                var firstTime = DateTime.Now;

                Parallel.For(0, count, action);
                var timeSpan = DateTime.Now.Subtract(firstTime);

                csvHelper.WriteField(name);
                csvHelper.WriteField(count.ToString());
                csvHelper.WriteField(GetTime(timeSpan));
                csvHelper.NextRecord();

                Assert.AreEqual(this._count, count);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message + " " + e.InnerException?.Message);
            }
        }

        private string GetTime(TimeSpan timeSpan)
        {
            if (timeSpan.Hours != 0)
            {
                return $"{timeSpan.Hours} hour";
            }

            if (timeSpan.Minutes != 0)
            {
                return $"{timeSpan.Minutes} min";
            }

            if (timeSpan.Seconds != 0)
            {
                return $"{timeSpan.Seconds} sec";
            }

            return $"{timeSpan.Milliseconds} ms";
        }

        private PatientUser GetLastPatientWithEntries()
        {
            return context.Patients.Include(x => x.Entries).ToList().Where(x => x.Entries != null && x.Entries.Any()).ToList().Last();
        }

        private void RemoveDoctors()
        {
            var doctors = context.Doctors.Include(x => x.PatientUsers).ToList().Where(x => x.UserName.StartsWith("doctor"));
            foreach (var source in doctors)
            {
                source.PatientUsers.Clear();
                context.Doctors.Remove(source);
                context.SaveChanges();
            }
        }

        private bool RemovDoctorsAndCheck(string patientId)
        {
            var doctors = context.Doctors.Include(x => x.PatientUsers).ToList().Where(x => x.UserName.StartsWith("doctor"));
            bool check = doctors.Count() == count;
            foreach (var source in doctors)
            {
                check &= source.PatientUsers.Any(x => x.Id == patientId);
                source.PatientUsers.Clear();
                context.Doctors.Remove(source);
                context.SaveChanges();
            }
            return check;
        }

        private List<string> GenerateDoctors()
        {
            var list = new List<string>();
            var userMananger = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
            for (int index = 0; index < count; index++)
            {
                var doctorUser = new DoctorUser() { Email = $"doctor{index}@gmail.com", DateOfBirth = DateTime.Now };
                doctorUser.UserName = doctorUser.Email;
                userMananger.Create(doctorUser);
                userMananger.AddToRole(doctorUser.Id, Roles.Doctor);

                list.Add(doctorUser.Id);
            }
            context.SaveChanges();
            return list;
        }


        private DoctorUser GetLastDoctor()
        {

            return context.Doctors.Include(x => x.PatientUsers)
                                          .ToList()
                                          .Where(x => x.PatientUsers != null && x.PatientUsers.Any())
                                          .ToList()
                                          .Last();
        }

        private void PerformTesting(string name, Func<bool> action)
        {
            this.PerformTesting(name, Wrapper(action));
        }

        private void PerformTesting(string name, Func<int, bool> action)
        {
            this.PerformTesting(name, Wrapper(action));
        }

        private Action<int> Wrapper(Func<int, bool> action)
        {
            return (index) =>
            {
                var resutl = action.Invoke(index);
                Assert.IsTrue(resutl);
            };
        }
        private void Count()
        {
            lock (lockObject)
            {
                _count++;
            }
        }

        private Action<int> Wrapper(Func<bool> action)
        {
            return (index) =>
                {
                    var invoke = action.Invoke();
                    Assert.IsTrue(invoke);
                };
        }

        private ApplicationDataProvider CreateSut()
        {
            return new ApplicationDataProvider(new ApplicationDbContext());
        }
    }
}
