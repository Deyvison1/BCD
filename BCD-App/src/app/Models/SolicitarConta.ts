import { Endereco } from './Endereco';

export class SolicitarConta {

    nomeConta: string;
    cpf: string;
    tipoConta: number;
    senha: string;
    enderecos: Endereco[];
    cep: number;
}