using System;
using System.Collections.Generic;
using System.Text;

namespace RSA
{
    class RSA
    {
        long e;
        long d;
        long n;

        bool[] nbPrem;
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="p">élément p de n</param>
        /// <param name="q">élément q de n</param>
        /// <param name="e"></param>
        public RSA(long p, long q, long e)
        {
            long z = (p - 1) * (q - 1);
            n = p * q;
            NbPremiers();
            if (nbPrem[p] == false)
            {
                Console.WriteLine("Clé invalide: {0} non premier", p);
                return; // gérer une exception
            }
            if (nbPrem[q] == false)
            {
                Console.WriteLine("Clé invalide: {0} non premier", q);
                return; // gérer une exception
            }
            if (SontPremiers(e, z) == false)
            {
                Console.WriteLine("Clé invalide: {0} non premier avec {1}", e, z);
                return;
            }
            // d = e^(phi(z)-1) mod z
            this.e = e;
            d = PuissanceMod(e, Phi(z) - 1, z);
            Console.WriteLine("Clé chiffrt valide ({0},{1})", e, n);
            Console.WriteLine("Clé déchrt: ({0},{1})", d, n);
        }


        /// <summary>
        /// Chiffrement d'un entier long
        /// </summary>
        /// <param name="val">valeur à chiffrer</param>
        /// <returns>valeur chiffrée</returns>
        public long Chiffre(long val)
        {
            return PuissanceMod(val, e, n);
        }

        /// <summary>
        /// Chiffrement d'une chaine de caractère
        /// </summary>
        /// <param name="str">Chaine de caractère à chiffrer</param>
        /// <returns></returns>
        public List<long> Chiffre(string str)
        {
            List<long> L = new List<long>();
            L = Decoupe(CodeChiffre(str));
            List<long> res = new List<long>();
            foreach (long i in L)
            {
                res.Add(Chiffre(i));
            }
            return res;
        }

        /// <summary>
        /// Chiffrement d'une chaine de caractère
        /// </summary>
        /// <param name="str">Chaine de caractère à chiffrer</param>
        /// <returns></returns>
        public string CodeChiffre(string str)
        {
            string res = "";
            int i;

            foreach (char c in str)
            {
                if (c == ' ')
                    i = 0;
                else
                    i = c - 'A' + 1;
                if (i < 10)
                    res = res + "0" + i.ToString();
                else
                    res = res + i.ToString();
            }
            return res;
        }
        
       /// <summary>
       /// Déchiffrement d'une chaine de caractère
       /// </summary>
       /// <param name="str">Chaine de caractère à déchiffrer</param>
       /// <returns></returns>
       public string DechiffreChaine(string str)
        {
            string res = "";
            int i;

            foreach (char c in str)
            {
                if (i == 0)
                    c = ' ';
                else
                    c = i + 'A' - 1;
                if (i > 10)
                    res = res + i.ToString();
            }
            return res;
        }

        /// <summary>
        /// Découpe la suite d'entiers en bloc
        /// </summary>
        /// <param name="str">Chaîne à découper en bloc</param>
        /// <returns></returns>
        public List<long> Decoupe(string str)
        {
            List<long> bloc = new List<long>();
            int longueur = n.ToString().Length - 1;
            int pos = 0;
            string aCoder;
            while (pos < str.Length - longueur)
            {
                aCoder = str.Substring(pos, longueur);
                bloc.Add(long.Parse(aCoder));
                pos += longueur;
            }
            //traitement du dernier bloc
            aCoder = str.Substring(pos, str.Length - pos);
            while (aCoder.Length < longueur)
                aCoder += "0";
            bloc.Add(long.Parse(aCoder));

            return bloc;
        }

        /// <summary>
        /// Déchiffrement d'un entier long
        /// </summary>
        /// <param name="val">valeur à déchiffrer</param>
        /// <returns>valeur déchiffrée</returns>
        public long Dechiffre(long val)
        {
            return PuissanceMod(val, d, n);
        }

        /// <summary>
        /// liste des nb premiers inférieurs à n
        /// </summary>
        private void NbPremiers()
        {
            nbPrem = new bool[n];

            for (long i = 0; i < n; i++)
                nbPrem[i] = true;

            nbPrem[0] = false;
            nbPrem[1] = false;

            for (long i = 2; i <= Math.Sqrt(n); i++)
                if (nbPrem[i] == true)
                    for (long mult = 2 * i; mult < n; mult = mult + i)
                        nbPrem[mult] = false;

            /*for (long i = 2; i < n; i++)
                if (nbPrem[i])
                    Console.Write("{0} ", i);*/
        }

        /// <summary>
        /// Indique si deux nb sont premiers entre eux
        /// </summary>
        /// <param name="x">nombre 1</param>
        /// <param name="y">nombre 2</param>
        /// <returns>true si x et y sont premiers,
        /// false sinon</returns>
        private bool SontPremiers(long x, long y)
        {
            long a, b, r;

            if (x > y)
            { a = x; b = y; }
            else
            { a = y; b = x; }

            while ((r = a % b) != 0)
            {
                a = b;
                b = r;
            }
            if (b == 1)    // Si le PGCD vaut 1
                return true;
            else
                return false;
        }

        /// <summary>
        /// Calcul de l'indicateur d'Euler
        /// </summary>
        /// <param name="val"></param>
        /// <returns>Indicateur d'Euler de val</returns>
        public long Phi(long val)
        {
            List<int> facteur = new List<int>();
            List<int> expo = new List<int>();
            int nbP = 2, exp;
            // Construction de la liste des n premiers nb premiers
            NbPremiers();

            while (val != 1)
            {
                if (val % nbP == 0)
                {
                    exp = 0;
                    facteur.Add(nbP);
                    while (val % nbP == 0)
                    {
                        exp++;
                        val = val / nbP;
                    }
                    expo.Add(exp);
                }
                // Nb premier suivant
                while (nbPrem[++nbP] == false) ;
            }
            double phi = 1;
            // Calcul de l'indicateur d'Euler
            for (int i = 0; i < facteur.Count; i++)
                phi = phi * Math.Pow(facteur[i], expo[i] - 1) *
                    (facteur[i] - 1);

            return (long)phi;
        }

        /// <summary>
        /// Puissance modulo n
        /// </summary>
        /// <param name="fact">facteur</param>
        /// <param name="exp">exposant</param>
        /// <param name="n">modulo</param>
        /// <returns>fact**exp modulo n</returns>
        /// 
        private long PuissanceMod(long fact, long exp, long n)
        {
            string binaire = Convert.ToString(exp, 2);
            long res = 1;

            for (int i = binaire.Length - 1; i >= 0; i--)
            {
                if (binaire[i] == '1')
                    res = (res * fact) % n;

                fact = fact * fact % n;
            }
            return res;
        }
        /// <summary>
        /// Puissance modulo n
        /// </summary>
        /// <param name="fact">facteur</param>
        /// <param name="exp">exposant</param>
        /// <param name="n">modulo</param>
        /// <returns>fact**exp modulo n</returns>
    }
}
