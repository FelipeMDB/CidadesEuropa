using System;

public class ListaSimples<Dado> where Dado : IComparable<Dado>
{
    private NoLista<Dado> primeiro, ultimo, anterior, atual;
    int quantosNos;

    private bool primeiroAcessoDoPercurso;

    public ListaSimples()
    {
        primeiro = ultimo = anterior = atual = null;
        quantosNos = 0;
        primeiroAcessoDoPercurso = false;
    }

    public void IniciarPercursoSequencial()
    {
        atual = primeiro;
        anterior = null;
        primeiroAcessoDoPercurso = true;
    }

    public bool PodePercorrer()
    {
        bool retorno = true;
        if (atual == null)
            retorno = false;
        else
        if (primeiroAcessoDoPercurso)
            primeiroAcessoDoPercurso = false;
        else
        {
            anterior = atual;
            atual = atual.Prox;
            if (atual == null)
                retorno = false;
        }

        return retorno;
    }
    public void percorrerLista()
    {
        atual = primeiro;
        while (atual != null)
        {
            Console.WriteLine(atual.Info);
            atual = atual.Prox;
        }
    }

    public bool EstaVazia
    {
        get => primeiro == null;
    }
    public NoLista<Dado> Primeiro { get => primeiro; }
    public NoLista<Dado> Ultimo { get => ultimo; }
    public int QuantosNos { get => quantosNos; }
    public NoLista<Dado> Atual { get => atual; set => atual = value; }

    public void InserirAntesDoInicio(NoLista<Dado> novoNo)
    {
        if (EstaVazia)
            ultimo = novoNo;
        novoNo.Prox = primeiro;
        primeiro = novoNo;
        quantosNos++;
    }

    public void InserirAntesDoInicio(Dado informacao)
    {
        if (informacao != null)
        {
            var novoNo = new NoLista<Dado>(informacao, null);
            InserirAntesDoInicio(novoNo);
        }
    }

    public void InserirAposFim(NoLista<Dado> novoNo)
    {
        if (EstaVazia)
            primeiro = novoNo;
        else
            ultimo.Prox = novoNo;
        novoNo.Prox = null;
        ultimo = novoNo;
        quantosNos++;
    }

    public void InserirAposFim(Dado informacao)
    {
        if (informacao != null)
        {
            var novoNo = new NoLista<Dado>(informacao, null);
            InserirAposFim(novoNo);
        }
    }


    public bool ExisteDado(Dado outroProcurado)
    {
        anterior = null;
        atual = primeiro;

        // Em seguida, é verificado se a lista está vazia. Caso esteja, é
        // retornado false ao local de chamada, indicando que a chave não foi
        // encontrada, e atual e anterior ficam valendo null

        if (EstaVazia)
            return false;

        // a lista não está vazia, possui nós
        // dado procurado é menor que o primeiro dado da lista:
        // portanto, dado procurado não existe

        if (outroProcurado.CompareTo(primeiro.Info) < 0)
            return false;

        // dado procurado é maior que o último dado da lista:
        // portanto, dado procurado não existe

        if (outroProcurado.CompareTo(ultimo.Info) > 0)
        {
            anterior = ultimo;
            atual = null;
            return false;
        }

        // caso não tenha sido definido que a chave está fora dos limites de
        // chaves da lista, vamos procurar no seu interior

        // o apontador atual indica o primeiro nó da lista e consideraremos que
        // ainda não achou a chave procurada nem chegamos ao final da lista

        bool achou = false;
        bool fim = false;

        // repete os comandos abaixo enquanto não achou o RA nem chegou ao
        // final da lista
        while (!achou && !fim)
        {
            // se o apontador atual vale null, indica final da lista
            if (atual == null)
                fim = true;

            // se não chegou ao final da lista, verifica o valor da chave atual
            else
            // verifica igualdade entre chave procurada e chave do nó atual
            if (outroProcurado.CompareTo(atual.Info) == 0)
                achou = true;
            else
            // se chave atual é maior que a procurada, significa que
            // a chave procurada não existe na lista ordenada e, assim,
            // termina a pesquisa indicando que não achou. Anterior
            // aponta o anterior ao atual, que foi acessado por
            // último
            if (atual.Info.CompareTo(outroProcurado) > 0)
                fim = true;
            else
            {
                // se não achou a chave procurada nem uma chave > que ela,
                // então a pesquisa continua, de maneira que o apontador
                // anterior deve apontar o nó atual e o apontador atual
                // deve seguir para o nó seguinte
                anterior = atual;
                atual = atual.Prox;
            }
        }
        // por fim, caso a pesquisa tenha terminado, o apontador atual
        // aponta o nó onde está a chave procurada, caso ela tenha sido
        // encontrada, ou o nó onde ela deveria estar para manter a
        // ordenação da lista. O apontador anterior aponta o nó anterior
        // ao atual
        return achou; // devolve o valor da variável achou, que indica
                      // se a chave procurada foi ou não encontrado
    }

    public void InserirEmOrdem(Dado dados)
    {
        if (!ExisteDado(dados)) // existeDado configurou anterior e atual
        {                       // aqui temos certeza de que a chave não existe
            NoLista<Dado> novo = new NoLista<Dado>(dados, null); // guarda dados no
                                                                 // novo nó
            if (EstaVazia) // se a lista está vazia, então o
                InserirAntesDoInicio(novo); // novo nó é o primeiro da lista
            else
            if (anterior == null && atual != null)
                InserirAntesDoInicio(novo); // liga novo antes do primeiro
            else
                InserirNoMeio(novo); // insere entre os nós anterior e atual
        }
    }
    private void InserirNoMeio(NoLista<Dado> novo)
    {
        // existeDado() encontrou intervalo de inclusão do novo nó
        anterior.Prox = novo; // liga anterior ao novo
        novo.Prox = atual; // e novo no atual
        if (anterior == ultimo) // se incluiu ao final da lista,
            ultimo = novo; // atualiza o apontador ultimo
        quantosNos++; // incrementa número de nós da lista
    }

    public bool Remover(Dado dados)
    {
        if (!ExisteDado(dados))  // ajusta ponteiros atual e anterior
            return false;

        // aqui, temos certeza de que a lista não está vazia, 
        // que o dado procurado existe, e seu
        // nó é apontado pelo atributo atual. 
        // O nó anterior é apontado pelo atributo anterior.

        RemoverNo(anterior, atual);

        return true;
    }
    protected void RemoverNo(NoLista<Dado> anterior, NoLista<Dado> atual)
    {
        if (anterior == null && atual != null) // o procurado é o 1o nó
        {
            primeiro = atual.Prox;
            if (primeiro == null) // lista ficou vazia
                ultimo = null;      // ajustamos ultimo para não ficar lixo
        }
        else
        {
            anterior.Prox = atual.Prox;
            if (atual == ultimo)
                ultimo = anterior;
        }
        quantosNos--;
    }

    private void ProcurarMenor(ref NoLista<Dado> antM,
                               ref NoLista<Dado> atuM)
    {
        antM = anterior = null;
        atuM = atual = primeiro;
        while (atual != null)
        {
            if (atual.Info.CompareTo(atuM.Info) < 0)
            {
                antM = anterior;
                atuM = atual;
            }
            anterior = atual;
            atual = atual.Prox;
        }
    }
    public void Ordenar()
    {
        NoLista<Dado> menor = null, antMenor = null, noAIncluir = null;
        var listaOrdenada = new ListaSimples<Dado>();
        while (!this.EstaVazia)
        {
            ProcurarMenor(ref antMenor, ref menor);
            noAIncluir = menor;  // reaproveitamos os nós da lista original
            this.RemoverNo(antMenor, menor);
            listaOrdenada.InserirAposFim(noAIncluir);
        }
        this.primeiro = listaOrdenada.primeiro;
        this.ultimo = listaOrdenada.ultimo;
        this.quantosNos = listaOrdenada.quantosNos;
        this.atual = this.anterior = null;
    }

    //////////////////////////////////////////////////////////////////
    // Exercícios
    //////////////////////////////////////////////////////////////////

    // Exercício 1: percorrer e contar nós
    public int Contar()
    {
        int quantos = 0;
        atual = primeiro;
        while (atual != null)
        {
            quantos++;
            atual = atual.Prox;
        }
        return quantos;
    }
    // Exercicio 3 : unir duas listas ligadas numa terceira

    public ListaSimples<Dado> UnirCom(ListaSimples<Dado> outra)
    {
        ListaSimples<Dado> novaLista = new ListaSimples<Dado>();
        atual = primeiro;
        outra.atual = outra.primeiro;
        while (atual != null && outra.atual != null)
        {
            if (atual.Info.CompareTo(outra.atual.Info) < 0)
            {
                novaLista.InserirAposFim(atual.Info);
                atual = atual.Prox;
            }
            else
              if (outra.atual.Info.CompareTo(atual.Info) < 0)
            {
                novaLista.InserirAposFim(outra.atual.Info);
                outra.atual = outra.atual.Prox;
            }
            else
            {
                novaLista.InserirAposFim(atual.Info);
                outra.atual = outra.atual.Prox;
                atual = atual.Prox;
            }

        }

        // se a lista this ainda não foi percorrida até o final,
        // terminamos de percorrê-la e incluímos os elementos 
        // faltantes na lista de união (novaLista)
        while (atual != null)
        {
            novaLista.InserirAposFim(atual.Info);
            atual = atual.Prox;
        }

        // se a outra lista ainda não foi percorrida até o final,
        // terminamos de percorrê-la e incluímos os elementos 
        // faltantes na lista de união (novaLista)
        while (outra.atual != null)
        {
            novaLista.InserirAposFim(outra.atual.Info);
            outra.atual = outra.atual.Prox;
        }

        return novaLista;
    }
    // Exercício 4 : inverter uma lista ligada

    public void Inverter()
    {
        if (!EstaVazia)
        {
            NoLista<Dado> um, dois, tres;
            um = primeiro;
            dois = um.Prox;
            while (dois != null)
            {
                tres = dois.Prox;
                dois.Prox = um;
                um = dois;
                dois = tres;
            }
            ultimo = primeiro;
            primeiro = um;
            ultimo.Prox = null;
        }
    }
}

