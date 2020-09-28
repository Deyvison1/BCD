import { Historico } from './Historico';

export class Conta {
    id: number; 
    digitosConta: number; 
    digitosAgencia: number; 
    tipoConta: number; 
    nomeConta: string;
    saldo: number;
    pessoaId: number; 
    extrato: Historico[];
    cpf: string;
}