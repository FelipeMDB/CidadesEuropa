using System;
using System.Collections;
using System.Collections.Generic;

class BucketHash
{
    public const int SIZE = 103;
    ListaSimples<Cidade>[] data;
    
    public BucketHash()
    {
        data = new ListaSimples<Cidade>[SIZE];
        for (int i = 0; i < SIZE; i++)
            data[i] = new ListaSimples<Cidade>();
    }

    public int Hash(string s)
    {
        long tot = 0;
        char[] charray;
        charray = s.ToUpper().ToCharArray();
        for (int i = 0; i <= s.Length - 1; i++)
            tot += 37 * tot + (int)charray[i];
        tot = tot % data.GetUpperBound(0);
        if (tot < 0)
            tot += data.GetUpperBound(0);
        return (int)tot;
    }

    public void Insert(Cidade c)
    {
        int hash_value = Hash(c.NomeCidade.ToUpper());
        if (!data[hash_value].ExisteDado(c))
            data[hash_value].InserirAposFim(c);
    }
    public bool Remove(Cidade c)
    {
        int hash_value = Hash(c.NomeCidade);
        if (data[hash_value].ExisteDado(c))
        {
            data[hash_value].Remover(c);
            return true;
        }
        return false;
    }

    public ListaSimples<Cidade> GetPosicao(int hash)
    {
        return data[hash];
    }
}