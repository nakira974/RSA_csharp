using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RSA;

namespace RSA
{
    class Program
    {
        static void Main(string[] args)
        {
            string message_chiffre =  "";
            string message_dechiffre = "";
            string chaine = "";
            long chiffreNum;
            RSA rsa = new RSA(503,563,565);
            //rsa.Phi(10403);
            Console.WriteLine("Saisir une chaine :");
            chaine = Console.ReadLine();            
            message_chiffre = rsa.CodeChiffre(chaine);
            Console.WriteLine("Chaine claire en chiffres : {0}", message_chiffre);
            Console.WriteLine("Découpage en blocs de longueur < n");
            message_chiffre = rsa.Decoupe(message_chiffre);
            Console.WriteLine(message_chiffre);
            Console.WriteLine("Message chiffré {0} :" , chaine);
            chiffreNum = rsa.Chiffre(message_chiffre);
            message_dechiffre = rsa.DechiffreChaine(chaine);
            Console.WriteLine("Chaine déchiffrée : {0} " , message_dechiffre);
            Console.ReadKey();
        }
    }
}
