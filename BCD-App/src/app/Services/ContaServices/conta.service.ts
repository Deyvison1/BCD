import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Conta } from 'src/app/Models/Conta';
import { HelperConta } from 'src/app/Models/HelperConta';
import { SolicitarConta } from 'src/app/Models/SolicitarConta';

@Injectable({
  providedIn: 'root'
})
export class ContaService {

  baseUrl = 'http://localhost:5000/api/conta/';

  constructor(
    private http: HttpClient
  ) { }


  getAllContasCadastradas(pessoaId: number): Observable<Conta[]> {
    return this.http.get<Conta[]>(`${this.baseUrl}contaCadastrada/${pessoaId}`);
  }

  // PEGAR MES ATUAL
  mesAtual(): Observable<number> {
    return this.http.get<number>(`${this.baseUrl}mesAtual`);
  }

  // GETS
  getAll(): Observable<Conta[]> {
    return this.http.get<Conta[]>(this.baseUrl);
  }

  getByContaAndAgencia(conta: number, agencia: number): Observable<Conta> {
    return this.http.get<Conta>(`${this.baseUrl}contaAndAgencia/${conta}/${agencia}`);
  }
  getById(id: number): Observable<Conta> {
    return this.http.get<Conta>(`${this.baseUrl}${id}`);
  }
  getBySearch(search: string): Observable<Conta[]> {
    return this.http.get<Conta[]>(`${this.baseUrl}buscar/${search}`);
  }
  getListByIdPessoa(idPessoa: number): Observable<Conta[]> {
    return this.http.get<Conta[]>(`${this.baseUrl}listarPorIdPessoa/${idPessoa}`);
  }
  // DELETE
  delete(id: number) {
    return this.http.delete(`${this.baseUrl}${id}`);
  }

  // SOLICITAR
  solicitarConta(solicitarConta: SolicitarConta) {
    return this.http.post(`${this.baseUrl}solicitar`, solicitarConta);
  }
  // DEPOSITO
  deposito(helperConta: HelperConta) {
    return this.http.put(`${this.baseUrl}deposito`, helperConta);
  }

  // RESGATAR VALOR
  resgatarValor(helperConta: HelperConta) {
    return this.http.put(`${this.baseUrl}resgatarPoupanca`, helperConta);
  }
  // APLICAR POUPANCA
  aplicarPoupanca(helperConta: HelperConta) {
    return this.http.put(`${this.baseUrl}aplicarPoupanca`, helperConta);
  }
  // TRANSFERENCIA
  transferencia(helperConta: HelperConta) {
    return this.http.put(`${this.baseUrl}transferencia`, helperConta);
  }

  // POST
  post(conta: Conta) {
    return this.http.post(this.baseUrl, conta);
  }

  // PUT
  put(conta: Conta) {
    return this.http.put(`${this.baseUrl}${conta.id}`, conta);
  }
  
}
