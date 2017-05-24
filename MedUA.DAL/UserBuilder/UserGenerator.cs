﻿namespace MedUA.DAL.UserBuilder
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Entity;
    using System.IO;
    using System.Linq;

    using MedUA.DAL.EntityModel;
    using MedUA.DAL.Migrations;
    using MedUA.DAL.TestData;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class UserGenerator
    {
        private readonly ApplicationDbContext context;

        private readonly ApplicationUserManager userManager;
        public UserGenerator(ApplicationDbContext context)
        {
            this.context = context;
            this.userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
        }

        public void Generate()
        {
            var entries = new EntriesGenerator().DoctorPosition();
            var placeOfBirth = TestDataGenerator.GetPlaces().ToList();
            this.RegisterRoles();
            this.RegisterResearches(TestDataGenerator.GetResearches());
            var femaleIds = this.RegisterPatients(TestDataGenerator.GetWomenNames(), placeOfBirth, MaleFemale.Female);
            var maleIds = this.RegisterPatients(TestDataGenerator.GetMenNames(), placeOfBirth, MaleFemale.Male);
            var count = entries.Count / 2;
            var list = femaleIds.Take(count).ToList();
            list.AddRange(maleIds.Take(count));
            this.RegisterDoctors(entries, placeOfBirth, list);
        }

        private void RegisterRoles()
        {
            if (!context.Roles.Any(x => x.Name == Roles.Doctor))
            {
                context.Roles.Add(new IdentityRole() { Name = Roles.Doctor });
            }
            if (!context.Roles.Any(x => x.Name == Roles.Patient))
            {
                context.Roles.Add(new IdentityRole() { Name = Roles.Patient });
            }
        }

        private void RegisterResearches(IEnumerable<string> researches)
        {
            foreach (var research in researches)
            {
                if (this.context.Researches.FirstOrDefault(x=>x.Name == research) != null) continue;
                var researchSave = new Research() { Name = research };
                this.context.Researches.Add(researchSave);
                this.context.SaveChanges();
            }
        }

        private IList<string> RegisterDoctors(Dictionary<string, Entry[]> doctorList, IList<string> places, IList<string> ids)
        {
            var doctrorUserBuilder = new DoctorUserBuilder(this.context, this.userManager);
            var list = new List<string>();
            var index = 0;
            var random = new Random();
            foreach (var doctor in doctorList)
            {
                var id = ids[index];
                var patient = userManager.FindById(id);
                var doctorUser = new DoctorUser()
                {
                    Name = patient.Name,
                    Surname = patient.Surname,
                    Partonimic = patient.Partonimic,
                    PlaceOfBirth = patient.PlaceOfBirth,
                    DateOfBirth = patient.DateOfBirth,
                    Code = patient.Code,
                    MaleFemale = patient.MaleFemale,
                    Email = patient.Email.Replace("gmail.com", "med.ua"),
                    Position = doctor.Key,
                    CurrentHospital = new Hospital() { Address = $"{places[random.Next(5)]}, {random.Next(1, 100)}",
                        Name = $"Лікарня № {random.Next(1, 100)}" }

                };
                doctorUser.UserName = doctorUser.Email;
                list.Add(doctrorUserBuilder.AddOrUpdate(doctorUser).Id);
                AddOrUpdatesEntries(doctorUser, doctorList.Keys.ToArray(), doctor.Value);
                index++;

            }
            return list;
        }


        private void AddOrUpdatesEntries(DoctorUser doctorUser, string[] doctorsIds, IEnumerable<Entry> entries)
        {
            if (this.context.Entries.Count(x => x.Doctor.Id == doctorUser.Id) > 1)
            {
                return;
            }
            foreach (var entry in entries)
            {
                entry.Doctor = doctorUser;
                var patient = this.context.Patients.Include(x => x.Doctors).ToList().First(x => (x.Doctors == null || !x.Doctors.Any()) && !doctorsIds.Contains(x.Id));
                var patientDb = this.context.Patients.Find(patient.Id);
                patientDb.Entries = patientDb.Entries ?? new List<Entry>();
                patientDb.Entries.Add(entry);
                patientDb.Doctors = new List<DoctorUser> { doctorUser };
                this.context.SaveChanges();
            }
        }

        private IList<string> RegisterPatients(IEnumerable<NamesAndAliases> namesAndAliaseses, IList<string> placeOfBirth, MaleFemale maleFemale)
        {
            var patientBuilder = new PatientUserBuilder(this.context, this.userManager);
            var random = new Random();
            var index = 0;
            var list = new List<string>();
            foreach (var nameAndAliase in namesAndAliaseses)
            {
                var patientUser = new PatientUser()
                {
                    Name = nameAndAliase.Name,
                    Surname = nameAndAliase.Surname,
                    Partonimic = nameAndAliase.Patronimic,
                    WeightAtBirth = random.Next(2, 6) + Math.Round(random.NextDouble(), 2),
                    BloodType = (BloodType)random.Next(8),
                    PlaceOfBirth = placeOfBirth[index],
                    DateOfBirth = GenerateRandomDateBirth(),
                    Code = DateTime.Now.Ticks.ToString(),
                    MaleFemale = maleFemale,
                    Email = $"{nameAndAliase.Alias}@gmail.com"
                };
                patientUser.UserName = patientUser.Email;
                list.Add(patientBuilder.AddOrUpdate(patientUser).Id);
                index++;
            }
            this.context.SaveChanges();

            return list;
        }

        
        private DateTime GenerateRandomDateBirth()
        {
            var random = new Random();
            var year = random.Next(1950, 1995);
            var month = random.Next(1, 12);
            var day = random.Next(1, 29);
            var hour = random.Next(24);
            var minute = random.Next(60);
            var second = random.Next(60);
            return new DateTime(year, month, day, hour, minute, second);
        }
    }
}