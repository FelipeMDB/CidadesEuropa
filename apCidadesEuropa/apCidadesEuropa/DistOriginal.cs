using apCidadesEuropa;

//Felipe Melchior de Britto  RA:18200
//Gabrielle da Silva Barbosa RA:18183
class DistOriginal
{
    public InformacoesPercurso peso; //contém tempo e distância
    public int verticePai; //para controlar o caminho por Dijkstra
    public DistOriginal(int vp, InformacoesPercurso i)
    {
        peso = i;
        verticePai = vp;
    }
}

