namespace Introduction.Logic.Data
{
    public static class StudentRepository
    {
        static StudentRepository()
        {
            LoadData();
        }

        private static string fileName = "Students.json";
        private static List<Models.Student> students = new();

        public static Task<Models.Student[]> GetAllAsync()
        {
            return Task.Run(() => students.ToArray());
        }

        public static Task<Models.Student?> GetByIdAsync(int id)
        {
            return Task.Run(() => students.FirstOrDefault(s => s.Id == id));
        }

        public static Task<Models.Student> CreateAsync()
        {
            return Task.Run(() => new Models.Student());
        }
        public static Task InsertAsync(Models.Student model)
        {
            var student = students.FirstOrDefault(s => s.MatriculationNumber.Equals(model.MatriculationNumber, StringComparison.CurrentCultureIgnoreCase));

            if (student != null)
            {
                throw new Exception($"{nameof(model.MatriculationNumber)} must be unique.");
            }
            if (students.Count() > 0)
            {
                model.Id = students.Max(s => s.Id) + 1;
            }
            return Task.Run(() => students.Add(model));
        }

        public static Task UpdateAsync(Models.Student model)
        {
            var student = students.FirstOrDefault(s => s.Id != model.Id && s.MatriculationNumber.Equals(model.MatriculationNumber, StringComparison.CurrentCultureIgnoreCase));

            if (student != null)
            {
                throw new Exception($"{nameof(model.MatriculationNumber)} must be unique.");
            }

            return Task.Run(() =>
            {
                var updateModel = students.FirstOrDefault(s => s.Id == model.Id);

                if (updateModel != null)
                {
                    updateModel.MatriculationNumber = model.MatriculationNumber;
                    updateModel.Firstname = model.Firstname;
                    updateModel.Lastname = model.Lastname;
                }
            });
        }

        public static Task DeleteAsync(int id)
        {
            return Task.Run(() =>
            {
                var idx = students.FindIndex(s => s.Id == id);

                if (idx >= 0)
                {
                    students.RemoveAt(idx);
                }
            });
        }

        public static Task SaveChangesAsync()
        {
            var jsonString = System.Text.Json.JsonSerializer.Serialize(students);

            return File.WriteAllTextAsync(fileName, jsonString);
        }

        private static void LoadData()
        {
            if (File.Exists(fileName))
            {
                var jsonString = File.ReadAllText(fileName);
                var items = System.Text.Json.JsonSerializer.Deserialize<Models.Student[]>(jsonString);

                students.Clear();

                if (items != null)
                {
                    students.AddRange(items);
                }
            }
            else
            {
                students.Add(new Models.Student
                {
                    MatriculationNumber = "IF20210001",
                    Firstname = "Hermann",
                    Lastname = "Huber",
                });
                students.Add(new Models.Student
                {
                    MatriculationNumber = "MT20210001",
                    Firstname = "Susi",
                    Lastname = "Traumfrau",
                });
                students.Add(new Models.Student
                {
                    MatriculationNumber = "IF20210002",
                    Firstname = "Ken",
                    Lastname = "Traummann",
                });
            }
        }
    }
}
