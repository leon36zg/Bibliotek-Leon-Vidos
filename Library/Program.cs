namespace Library
{
    internal class Program
    {
        public static void Main(string[] args)
        {   //Början av programmet
            Console.WriteLine("Välkommen till programmet!");
            Console.WriteLine("Välj en av alternativerna:"); 
            Console.WriteLine("Om du vill skapa konto,TRYCK 1");
            Console.WriteLine("Om du vill logga in, TRYCK 2\n"); 
            Console.WriteLine("Välj en siffra: ");
            string number = Console.ReadLine();


            if (number == "1")
            {

                Console.WriteLine("");
                RegisterUser();
            }

            else if (number == "2")
            {
                Console.WriteLine("");
                LogInPage();
            }

        }



        //Regristreringsida
        static void RegisterUser()
        {
            Console.WriteLine("Ange förnamn, efternamn, personnummer, och lösenord för att registrera");

            Console.Write("Förnamn: ");
            string firstName = Console.ReadLine();

            Console.Write("Efternamn: ");
            string lastName = Console.ReadLine();

            Console.Write("Personnummer: ");
            string personalNumber = Console.ReadLine();

            Console.Write("Lösenord: ");
            string password = Console.ReadLine();

            if (!IsUserInfoIncomplete(firstName, lastName, personalNumber, password))
            {
                if (!IsUserRegistered(firstName, lastName, personalNumber))
                {
                    //Användarens info skickas till databasen
                    string line = firstName + " " + lastName + " " + personalNumber + " " + password;
                    string dataForPn = personalNumber;
                    string dataForPassword = password;
                    File.AppendAllText(@"C:\Users\leon.vidos\source\repos\TestFörProg2Bibliotek\Library\users.txt", line + Environment.NewLine);
                    File.AppendAllText(@"C:\Users\leon.vidos\source\repos\TestFörProg2Bibliotek\Library\personal_numbers.txt", dataForPn + Environment.NewLine);
                    File.AppendAllText(@"C:\Users\leon.vidos\source\repos\TestFörProg2Bibliotek\Library\password.txt", dataForPassword + Environment.NewLine);



                    Console.WriteLine("Användaren är nu registrerad!");
                }
                else
                {
                    Console.WriteLine("Du är redan registrerad. Var vänlig och byt Personnummer.");
                }
            }
            else
            {
                Console.WriteLine("Ogiltig information. Var vänlig och ange alla nödvändiga uppgifter.");
            }

            Console.WriteLine("Du skickas till inloggningssidan, vänligen vänta! :-)");

            Thread.Sleep(2000); 

            LogInPage(); // skickas från profilsida till loginsida
        }


        static bool IsUserRegistered(string firstName, string lastName, string personalNumber)
        {
            string[] users = File.ReadAllLines(@"C:\Users\leon.vidos\source\repos\TestFörProg2Bibliotek\TestFörProg2Bibliotek\users.txt");
            foreach (string user in users)
            {
                string[] parts = user.Split(' ');
                if (parts[0] == firstName && parts[1] == lastName && parts[2] == personalNumber)
                {
                    return true;
                }
            }
            return false;
        }


        static bool IsUserInfoIncomplete(string firstName, string lastName, string personalNumber, string password)
        {
            return string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) ||
                   string.IsNullOrWhiteSpace(personalNumber) || string.IsNullOrWhiteSpace(password);
        }

        static bool Authenticate(string personalNumber, string password)
        {
            string[] personalNumbersFromDb = System.IO.File.ReadAllLines(@"C:\Users\leon.vidos\source\repos\TestFörProg2Bibliotek\Library\personal_numbers.txt");
            string[] passwordsFromDb = System.IO.File.ReadAllLines(@"C:\Users\leon.vidos\source\repos\TestFörProg2Bibliotek\Library\password.txt");

            if (personalNumbersFromDb.Length != passwordsFromDb.Length)
            {
                return false;
            }

            for (int i = 0; i < personalNumbersFromDb.Length; i++)
            {
                string personalNumberFromDb = personalNumbersFromDb[i];
                string passFromDb = passwordsFromDb[i];

                if (personalNumber == personalNumberFromDb && password == passFromDb)
                {
                    return true;
                }
            }

            return false;
        }

        static void LogInPage()  //loginsida 
        {
            
            bool wrongpassword = false;
            
            string personalNumber = "";
            
            string password = "";

            while (!Authenticate(personalNumber, password))
            {
                Console.Clear();

                if (wrongpassword)
                {
                    Console.WriteLine("Fel lösenord!");
                }
                else
                {
                    Console.WriteLine("Välkommen! :-)"); 
                }

                Console.WriteLine("För att logga in, ange personnummer och lösenord.");
                Console.WriteLine("");

                Console.Write("Personnummer: ");
                personalNumber = Console.ReadLine();

                Console.Write("Lösenord: ");
                password = Console.ReadLine();

                Console.WriteLine("");

                wrongpassword = true;
            }

            ProfilSida(personalNumber);
        }

        static void ProfilSida(string personalNumber) //efter login skickas till profilsida där du väkjer olika alternativ
        {
            Console.Clear();
            Console.WriteLine("ProfilSida\n");
            Console.WriteLine("Nu är du inloggad!");

            Console.WriteLine("1. Ändra lösenord");
            
            Console.WriteLine("2. Logga ut");
            
            Console.WriteLine("3. Söka böcker");
            
            Console.Write("Välj ett alternativ: "); 

            int option = int.Parse(Console.ReadLine());

            if (option == 1)
            {
                ChangePassword(personalNumber);
            } 
            if (option == 2)
            {
                Console.WriteLine("Du är nu utloggad.");
                Console.ReadKey();
            }
            else if (option == 3)
            {
                BookSearchProgram(); 
            }
        }

        static void ChangePassword(string personalNumber) //ändra lösenord
        {
            Console.Clear();
            Console.WriteLine("Ändra lösenord");

            string newPassword;

            do
            {
                Console.Write("Nytt lösenord (minst 8 tecken): ");
                newPassword = Console.ReadLine();

                if (newPassword.Length < 8)
                {
                    Console.WriteLine("Lösenordet måste vara minst 8 tecken långt.");
                }
            } while (newPassword.Length < 8);

            string[] personalNumbersFromDb = File.ReadAllLines(@"C:\Users\leon.vidos\source\repos\TestFörProg2Bibliotek\Library\personal_numbers.txt");
            string[] passwordsFromDb = File.ReadAllLines(@"C:\Users\leon.vidos\source\repos\TestFörProg2Bibliotek\Library\password.txt");
            string[] usersFromDb = File.ReadAllLines(@"C:\Users\leon.vidos\source\repos\TestFörProg2Bibliotek\Library\users.txt");

            for (int i = 0; i < personalNumbersFromDb.Length; i++)
            {
                if (personalNumbersFromDb[i] == personalNumber)
                {
                    passwordsFromDb[i] = newPassword;
                    usersFromDb[i] = personalNumber + "," + newPassword;
                    File.WriteAllLines(@"C:\Users\leon.vidos\source\repos\TestFörProg2Bibliotek\Library\password.txt", passwordsFromDb);
                    File.WriteAllLines(@"C:\Users\leon.vidos\source\repos\TestFörProg2Bibliotek\Library\users.txt", usersFromDb);
                    Console.WriteLine("Lösenordet har ändrats.");
                    Console.ReadKey();
                    return;
                }
            }

            Console.WriteLine("Det var något som gick fel! :-(");
            Console.ReadKey();
        } 
        
        static void BookSearchProgram()  
        {
            // böckerna som man kan söka på 
            List<Book> books = new List<Book>();
            books.Add(new Book("I am Zlatan Ibrahomovic", "David Lagercrantz", "Månpocket", 2011));
            books.Add(new Book("En halv gul sol", "Chimamanda Ngozi Adichie", "Anchor", 2006));
            books.Add(new Book("1984", "George Orwell", "Secker & Warburg", 1949));
            books.Add(new Book("Harry Potter och dödsrelikerna", "J.K. Rowling", "Times", 2007));
            books.Add(new Book("One Hundred Years of Solitude", "Gabriel Garcia Marquez", "Harper & Row", 1967));
            books.Add(new Book("Män som hatar kvinnor", "Stieg Larsson", "Bokus", 2005));
            books.Add(new Book("Ungdomsår", "J.M. Coetzee", "INBUNDEN", 2002));
            books.Add(new Book("Fun Home", "Alison Bechdel", "BR Records", 2006)); 

            Console.WriteLine("Ange en sökterm: (t.ex. boknamn, författare eller år):");
            string searchTerm = Console.ReadLine();

            List<Book> results = new List<Book>();
            foreach (Book book in books)
            {
                if (book.Title.ToLower().Contains(searchTerm.ToLower()) ||
                    book.Author.ToLower().Contains(searchTerm.ToLower()) ||
                    book.Publisher.ToLower().Contains(searchTerm.ToLower()) || 
                    book.Year.ToString().Contains(searchTerm))
                {
                    results.Add(book);
                }
            }

            Console.WriteLine($"Hitta {results.Count} bok(er) som matchar söktermen '{searchTerm}':"); 
            foreach (Book result in results)
            {
                Console.WriteLine($"Titel: {result.Title}, Författare: {result.Author}, Publiceraren: {result.Publisher}, År: {result.Year}");
            }

            Console.ReadLine();
        }

        class Book
        {
            public string Title { get; set; } 
            public string Author { get; set; }
            public string Publisher { get; set; }
            public int Year { get; set; }

            public Book(string title, string author, string publisher, int year) 
            {
                Title = title;
                Author = author;
                Publisher = publisher;
                Year = year;
            }
        } 
    }
} 

