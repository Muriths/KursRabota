namespace LibraryApp
{
    class Program
    {
        static string usersFilePath = "users.txt";
        static string booksFilePath = "books.txt";
        static string CurrentUser;

        static void Main()
        {
            Console.WriteLine("Добро пожаловать в Библиотеку!");

            bool isAuthorized = false;
            string currentUser = string.Empty;

            while (!isAuthorized)
            {
                Console.WriteLine("Введите логин:");
                string login = Console.ReadLine();

                Console.WriteLine("Введите пароль:");
                string password = Console.ReadLine();

                if (AuthenticateUser(login, password, out string role))
                {
                    isAuthorized = true;
                    currentUser = login;
                    Console.WriteLine($"Авторизация прошла успешно. Роль: {role}");
                }
                else
                {
                    Console.WriteLine("Ошибка авторизации. Попробуйте снова.");
                }
            }
            CurrentUser = currentUser;
                if (IsAdmin(currentUser))
                {
                    AdminMenu();
                }
                else
                {
                    UserMenu(currentUser);
                }
        }

        static void ReturnToMenu()
        {
            Console.Clear();
            if (IsAdmin(CurrentUser))
            {
                AdminMenu();
            }
            else
            {
                UserMenu(CurrentUser);
            }
        }


        static bool AuthenticateUser(string login, string password, out string role)
        {
            string[] users = File.ReadAllLines(usersFilePath);
            foreach (string user in users)
            {
                string[] userInfo = user.Split(':');
                string userLogin = userInfo[0].Trim();
                string userPassword = userInfo[1].Trim();
                role = userInfo[2].Trim();

                if (userLogin == login && userPassword == password)
                {
                    return true;
                }
            }
            
            role = string.Empty;
            return false;
        }

        static bool IsAdmin(string username)
        {
            string[] users = File.ReadAllLines(usersFilePath);
            foreach (string user in users)
            {
                string[] userInfo = user.Split(':');
                string userLogin = userInfo[0].Trim();
                string userRole = userInfo[2].Trim();

                if (userLogin == username && userRole == "admin")
                {
                    return true;
                }
            }

            return false;
        }

        static void AdminMenu()
        {
            Console.WriteLine("Вы вошли как администратор.");
            Console.WriteLine("Выберите действие:");
            Console.WriteLine("1. Удалить пользователя");
            Console.WriteLine("2. Создать пользователя");
            Console.WriteLine("3. Просмотреть понравившиеся книги пользователей");
            Console.WriteLine("4. Предоставить права администратора пользователю");
            Console.WriteLine("5. Просмотреть список книг");
            Console.WriteLine("6. Сменить пользователя");
            Console.WriteLine("7. Выйти");

            int choice = GetChoice(1, 7);

            switch (choice)
            {
                case 1:
                    DisplayAllUsers();
                    Console.WriteLine("Введите логин пользователя, которого нужно удалить:");
                    string userToDelete = Console.ReadLine();
                    if (DeleteUser(userToDelete))
                    {
                        Console.WriteLine("Пользователь успешно удален.");
                    }
                    else
                    {
                        Console.WriteLine("Ошибка при удалении пользователя.");
                    }
                    Console.ReadKey();
                    ReturnToMenu();
                    break;

                case 2:
                    Console.WriteLine("Введите логин нового пользователя:");
                    string newUsername = Console.ReadLine();
                    Console.WriteLine("Введите пароль нового пользователя:");
                    string newPassword = Console.ReadLine();
                    Console.WriteLine("Введите роль нового пользователя (admin/user):");
                    string newRole = Console.ReadLine();
                    if (CreateUser(newUsername, newPassword, newRole))
                    {
                        Console.WriteLine("Пользователь успешно создан.");
                    }
                    else
                    {
                        Console.WriteLine("Ошибка при создании пользователя.");
                    }
                    Console.ReadKey();
                    ReturnToMenu();
                    break;

                case 3:
                    DisplayAllUsers();
                    Console.WriteLine("Введите логин пользователя, чьи понравившиеся книги нужно просмотреть:");
                    string userToViewBooks = Console.ReadLine();
                    ViewLikedBooks(userToViewBooks);
                    Console.ReadKey();
                    ReturnToMenu();
                    break;

                case 4:
                    DisplayAllUsers();
                    Console.WriteLine("Введите логин пользователя, которому нужно предоставить права администратора:");
                    string userToGrantAdmin = Console.ReadLine();
                    if (GrantAdminRights(userToGrantAdmin))
                    {
                        Console.WriteLine("Права администратора успешно предоставлены.");
                    }
                    else
                    {
                        Console.WriteLine("Ошибка при предоставлении прав администратора.");
                    }
                    Console.ReadKey();
                    ReturnToMenu();
                    break;

                case 5:
                    ViewAllBooks();
                    Console.ReadKey();
                    ReturnToMenu();
                    break;
                case 6:
                    SwitchUser();
                    break;

                case 7:
                    Console.WriteLine("Выход...");
                    Environment.Exit(0);
                    break;
            }
        }

        static void UserMenu(string username)
        {
            Console.WriteLine($"Вы вошли как пользователь {username}.");
            Console.WriteLine("Выберите действие:");
            Console.WriteLine("1. Просмотреть список книг");
            Console.WriteLine("2. Просмотреть описание книги");
            Console.WriteLine("3. Добавить книгу в избранное");
            Console.WriteLine("4. Просмотреть отзывы по книге");
            Console.WriteLine("5. Написать отзыв о книге");
            Console.WriteLine("6. Сменить пользователя");
            Console.WriteLine("7. Выйти");

            int choice = GetChoice(1, 7);

            switch (choice)
            {
                case 1:
                    ViewAllBooks();
                    Console.ReadKey();
                    ReturnToMenu();
                    break;

                case 2:
                    ViewAllBooks();
                    Console.WriteLine("Введите номер книги для просмотра описания:");
                    int bookIndex = GetChoice(1, int.MaxValue);
                    ViewBookDescription(bookIndex);
                    Console.ReadKey();
                    ReturnToMenu();
                    break;

                case 3:
                    ViewAllBooks();
                    Console.WriteLine("Введите номер книги, чтобы добавить ее в избранное:");
                    int favoriteBookIndex = GetChoice(1, int.MaxValue);
                    AddToFavorites(username, favoriteBookIndex);
                    Console.ReadKey();
                    ReturnToMenu();
                    break;

                case 4:
                    ViewAllBooks();
                    Console.WriteLine("Введите номер книги для просмотра отзывов:");
                    int reviewBookIndex = GetChoice(1, int.MaxValue);
                    ViewBookReviews(reviewBookIndex);
                    Console.ReadKey();
                    ReturnToMenu();
                    break;

                case 5:
                    ViewAllBooks();
                    Console.WriteLine("Введите номер книги, для которой хотите написать отзыв:");
                    int reviewBookIndexToWrite = GetChoice(1, int.MaxValue);
                    Console.WriteLine("Введите отзыв:");
                    string review = Console.ReadLine();
                    if (WriteBookReview(username, reviewBookIndexToWrite, review))
                    {
                        Console.WriteLine("Отзыв успешно добавлен.");
                    }
                    else
                    {
                        Console.WriteLine("Ошибка при добавлении отзыва.");
                    }
                    Console.ReadKey();
                    ReturnToMenu();
                    break;
                case 6:
                    SwitchUser();
                    break;

                case 7:
                    Console.WriteLine("Выход...");
                    Environment.Exit(0);
                    break;
            }
        }

        static int GetChoice(int minValue, int maxValue)
        {
            int choice;
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out choice) && choice >= minValue && choice <= maxValue)
                {
                    return choice;
                }
                else
                {
                    Console.WriteLine($"Пожалуйста, введите число от {minValue} до {maxValue}.");
                }
            }
        }

        static void DisplayAllUsers()
        {
            Console.WriteLine("Список пользователей:");
            string[] users = File.ReadAllLines(usersFilePath);
            foreach (string user in users)
            {
                Console.WriteLine(user);
            }
        }

        static bool DeleteUser(string username)
        {
            List<string> users = new List<string>(File.ReadAllLines(usersFilePath));
            int index = -1;

            for (int i = 0; i < users.Count; i++)
            {
                if (users[i].StartsWith(username))
                {
                    index = i;
                    break;
                }
            }

            if (index != -1)
            {
                users.RemoveAt(index);
                if (File.Exists($"{username}_liked_books.txt")) //файл - file , exists - существует ли
                {
                    File.Delete($"{username}_liked_books.txt");
                }
                File.WriteAllLines(usersFilePath, users.ToArray());
                return true;
            }

            return false;
        }

        static bool CreateUser(string username, string password, string role)
        {
            string user = $"{username}:{password}:{role}";
            try
            {
                if (File.ReadAllLines(usersFilePath).Contains(username))
                {
                    throw new Exception("Такой пользователь уже существует !");
                }
                File.AppendAllText(usersFilePath, $"{Environment.NewLine}{user}");
                return true;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        static void ViewLikedBooks(string username)
        {
            string likedBooksFilePath = $"{username}_liked_books.txt";
            if (File.Exists(likedBooksFilePath))
            {
                Console.WriteLine($"Список понравившихся книг пользователя {username}:");
                string[] likedBooks = File.ReadAllLines(likedBooksFilePath);
                foreach (string book in likedBooks)
                {
                    Console.WriteLine(book);
                }
            }
            else
            {
                Console.WriteLine($"Нет данных о понравившихся книгах пользователя {username}.");
            }
        }

        static bool GrantAdminRights(string username)
        {
            string[] users = File.ReadAllLines(usersFilePath);
            for (int i = 0; i < users.Length; i++)
            {
                if (users[i].StartsWith(username))
                {
                    string[] userInfo = users[i].Split(':');
                    if (userInfo[2] != "admin")
                    {
                        userInfo[2] = "admin";
                        users[i] = string.Join(":", userInfo);
                        File.WriteAllLines(usersFilePath, users);
                        return true;
                    }
                    else if (userInfo[2] == "admin")
                    {
                        Console.WriteLine("Этот пользователь уже является администратором !");
                        return false;
                    }
                }
            }

            return false;
        }

        static void ViewAllBooks()
        {
            Console.WriteLine("Список всех книг:");
            string[] books = File.ReadAllLines(booksFilePath);
            foreach (string book in books)
            {
                Console.WriteLine(book);
            }
        }

        static void ViewBookDescription(int bookIndex)
        {
            string[] books = File.ReadAllLines(booksFilePath);
            Console.WriteLine("Список книг : ");
            foreach(string _book in books)
            {
                Console.WriteLine(_book);
            }
            if (bookIndex >= 1 && bookIndex <= books.Length)
            {
                string book = books[bookIndex - 1];
                Console.WriteLine($"Описание книги {bookIndex}: {book}.\n{File.ReadAllLines($"{book}_description.txt")}");
            }
            else
            {
                Console.WriteLine("Некорректный номер книги.");
            }
        }

        static void AddToFavorites(string username, int bookIndex)
        {
            string likedBooksFilePath = $"{username}_liked_books.txt";
            string[] books = File.ReadAllLines(booksFilePath);

            if (bookIndex >= 1 && bookIndex <= books.Length)
            {
                string bookToAdd = books[bookIndex - 1];
                try
                {
                    File.AppendAllText(likedBooksFilePath, $"{Environment.NewLine}{bookToAdd}");
                    Console.WriteLine("Книга успешно добавлена в избранное.");
                }
                catch
                {
                    Console.WriteLine("Ошибка при добавлении книги в избранное.");
                }
            }
            else
            {
                Console.WriteLine("Некорректный номер книги.");
            }
        }

        static void ViewBookReviews(int bookIndex)
        {
            Console.WriteLine("Выберите книгу, к которой хотите просмотреть отзывы");
            string[] books = File.ReadAllLines(booksFilePath);
            if (bookIndex >= 1 && bookIndex <= books.Length)
            {
                string book = books[bookIndex - 1];
                string bookReviewsFilePath = $"{bookIndex}_reviews.txt";

                if (File.Exists(bookReviewsFilePath))
                {
                    Console.WriteLine($"Отзывы по книге {bookIndex}:");
                    string[] reviews = File.ReadAllLines(bookReviewsFilePath);
                    foreach (string review in reviews)
                    {
                        Console.WriteLine(review);
                    }
                }
                else
                {
                    Console.WriteLine($"Нет отзывов по книге {bookIndex}.");
                }
            }
            else
            {
                Console.WriteLine("Некорректный номер книги.");
            }
        }

        static bool WriteBookReview(string username, int bookIndex, string review)
        {
            string[] books = File.ReadAllLines(booksFilePath);
            if (bookIndex >= 1 && bookIndex <= books.Length)
            {
                string bookReviewsFilePath = $"{bookIndex}_reviews.txt";
                string userReview = $"{username}: {review}";

                try
                {
                    File.AppendAllText(bookReviewsFilePath, $"{Environment.NewLine}{userReview}");
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Некорректный номер книги.");
                return false;
            }
        }
        static void SwitchUser()
        {
            Main();
            ReturnToMenu();
        }
    }
}
