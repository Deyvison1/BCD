import { Pessoa } from "./Pessoa";

export class Endereco {
  id: number;
  cep: number;
  logradouro: string;
  complemento: string;
  bairro: string;
  localidade: string;
  uf: string;
  ibge: number;
  gia: string;
  ddd: string;
  siafi: string;
  pessoas: Pessoa[];

  constructor() {
        this.pessoas = new Array();
  }
}
