using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTestExample.Abstractions;
using UnitTestExample.Entities;
using static System.Windows.Forms.LinkLabel;

//Az osztály biztosítja, hogy ne lehessen két ugyanolyan e-mail címmel rendelkező fiókot létrehozni, 
//és kezeli a felhasználói fiókok listáját, amelyhez a felhasználói felület is könnyen kapcsolható.

namespace UnitTestExample.Services
{
    public class AccountManager : IAccountManager /*osztály egy egyszerű felhasználókezelő szolgáltatás.*/
    {
        public BindingList<Account> Accounts { get; } = new BindingList<Account>();

        public Account CreateAccount(Account account) /*Ez a metódus felelős új felhasználói fiók létrehozásáért.*/
        {
            // Először ellenőrzi, hogy van - e már olyan felhasználó, akinek ugyanaz az e-mail címe, 
            // mint az újonnan létrehozandó fióknak.Ezt egy LINQ - lekérdezéssel teszi meg.
            var oldAccount = (from a in Accounts
                              where a.Email.Equals(account.Email) 
                              select a).FirstOrDefault(); 

            //•	Ha talál ilyet, kivételt dob egy magyar nyelvű hibaüzenettel: 
            //  "Már létezik felhasználó ilyen e-mail címmel!"
            if (oldAccount != null)
                throw new ApplicationException(
                    "Már létezik felhasználó ilyen e-mail címmel!");

            //Ha nincs ilyen felhasználó, hozzáadja az új fiókot az Accounts listához, majd visszaadja azt.
            Accounts.Add(account);

            return account;
        }
    }
}
