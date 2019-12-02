using System;
class Vertice
{
    public bool foiVisitado;
    public string rotulo;
    private bool estaAtivo;

    public Vertice(string label)
    {
        rotulo = label;
        foiVisitado = false;
        estaAtivo = true;
    }

}

