using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//Felipe Melchior de Britto  RA:18200
//Gabrielle da Silva Barbosa RA:18200
class Cidade : IComparable<Cidade>
{

    //declaração das variáveis que serão utilizadas para leitura de arquivo
    int idCidade;
    const int tamanhoIdCidade = 2;
    const int inicioIdCidade = 0;
    string nomeCidade;
    const int tamanhoNomeCidade = 16;
    const int inicioNomeCidade = tamanhoIdCidade + inicioIdCidade;
    double coordenadaX;
    const int tamanhoCoordenadaX = 6;
    const int inicioCoordenadaX = inicioNomeCidade + tamanhoNomeCidade;
    double coordenadaY;
    const int tamanhoCoordenadaY = 5;
    const int inicioCoordenadaY = inicioCoordenadaX + tamanhoCoordenadaX;


    //declaração das variáveis globais da classe 
    public int IdCidade { get => idCidade; set => idCidade = value; }
    public string NomeCidade { get => nomeCidade; set => nomeCidade = value; }
    public double CoordenadaX { get => coordenadaX; set => coordenadaX = value; }
    public double CoordenadaY { get => coordenadaY; set => coordenadaY = value; }

    //construtor da classe
    //parâmetros: id da cidade, nome da cidade, coordenada x, coordenada y
    public Cidade(int id, string nome, int x, int y)
    {
        idCidade = id;
        nomeCidade = nome;
        coordenadaX = x;
        coordenadaY = y;
    }

    //lê os arquivos  e os separa nas respectivas variáveis da classe retornando uma cidade
    public static Cidade LerArquivo(StreamReader arq)
    {
        string linha = arq.ReadLine();
        int idCidade = int.Parse(linha.Substring(inicioIdCidade, tamanhoIdCidade));
        string nomeCidade = linha.Substring(inicioNomeCidade, tamanhoNomeCidade).Trim();
        int coordenadaX = int.Parse(linha.Substring(inicioCoordenadaX, tamanhoCoordenadaX));
        int coordenadaY = int.Parse(linha.Substring(inicioCoordenadaY, tamanhoCoordenadaY));
        return new Cidade(idCidade, nomeCidade, coordenadaX, coordenadaY);
    }

    //método compareTo requisitado pela interface
    public int CompareTo(Cidade other)
    {
        //compara os ids das cidades
        return this.idCidade - other.idCidade;
    }
}
