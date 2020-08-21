import { Historico } from './Historico';

export class Conta {
    id: number; 
    digitosConta: number; 
    digitosAgencia: number; 
    tipoConta: EnumTipoContaDto; 
    nomeConta: string;
    saldo: number;
    pessoaId: number; 
    extrato: Historico[];
}