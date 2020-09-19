import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Conta } from 'src/app/Models/Conta';
import { HelperConta } from 'src/app/Models/HelperConta';

@Injectable({
  providedIn: 'root'
})
export class ContaService {

  baseUrl = 'http://localhost:5000/api/conta/';
  constructor(
    private http: HttpClient
  ) { }

  // GETS
  getAll(): Observable<Conta[]> {
    return this.http.get<Conta[]>(this.baseUrl);
  }

  getByContaAndAgencia(conta: number, agencia: number): Observable<Conta> {
    return this.http.get<Conta>(`${this.baseUrl}contaAndAgencia/${conta}/${agencia}`);
  }
  getAllDeleteNomeCurrency(nomeConta: string): Observable<Conta[]> {
    return this.http.get<Conta[]>(`${this.baseUrl}nomeConta/${nomeConta}`);
  }
  getById(id: number): Observable<Conta> {
    return this.http.get<Conta>(`${this.baseUrl}${id}`);
  }
  getBySearch(search: string): Observable<Conta[]> {
    return this.http.get<Conta[]>(`${this.baseUrl}buscar/${search}`);
  }
  getByIdList(id: number): Observable<Conta[]> {
    return this.http.get<Conta[]>(`${this.baseUrl}listar/${id}`);
  }
  // DELETE
  delete(id: number) {
    return this.http.delete(`${this.baseUrl}${id}`);
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
