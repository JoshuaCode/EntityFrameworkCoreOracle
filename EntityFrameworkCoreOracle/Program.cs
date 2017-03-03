using Devart.Data.Oracle.Entity.Configuration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace EntityFrameworkCoreOracle
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = OracleEntityProviderConfig.Instance;
            config.Workarounds.DisableQuoting = true;

            using (var db = new PersonContext())
            {
                Console.WriteLine("### Query 1");
                var persons = db.Persons.Include(person => person.Manager);
                foreach (var person in persons)
                {
                    Console.WriteLine($"Scid: {person.Scid} Name: {person.Name}  Manager: {person.Manager?.Name}");
                }
                Console.WriteLine("### Query 2");
                var personsList = db.Persons.Select(p => new Person()
                {
                    Name = p.Name,
                    Manager = new Person()
                    {
                        Name = p.Manager.Name
                    }
                });
                foreach (var person in personsList)
                {
                    Console.WriteLine($"Scid: {person.Scid} Name: {person.Name}  Manager: {person.Manager?.Name}");
                }
                Console.WriteLine("### Query 3");
                var directReports = db.Persons.Where(p => p.Scid == "00000001").Select(p => new Person() { Name = p.Name, DirectReports = p.DirectReports }).FirstOrDefault();
                foreach (var person in directReports.DirectReports)
                {
                    Console.WriteLine($"Manager: {directReports.Name} EmployeeName: {person.Name}");
                }
            }
        }
    }
}
