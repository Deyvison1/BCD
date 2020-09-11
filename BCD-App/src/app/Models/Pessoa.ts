import { Endereco } from './Endereco'
import { Conta } from './Conta'

export class Pessoa {
        id: number;
        nome: string;
        cpf: string;
        contas: Conta[];
        enderecos: Endereco[];
}