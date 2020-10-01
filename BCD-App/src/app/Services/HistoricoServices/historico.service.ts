import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Historico } from 'src/app/Models/Historico';

@Injectable({
  providedIn: 'root'
})
export class HistoricoService {

  baseURL = 'http://localhost:5000/api/historico/';

  constructor(
    private http: HttpClient
  ) { }


  getLastMeses(agencia: number, conta: number, tipoConta: number): Observable<Historico[]> {
    return this.http.get<Historico[]>(`${this.baseURL}listLast/${agencia}/${conta}/${tipoConta}`);
  }

  getByMesCorrente(mes: number, agencia: number, conta: number): Observable<Historico[]> {
    return this.http.get<Historico[]>(`${this.baseURL}listarPeloMesCorrente/${mes}/${agencia}/${conta}`);
  }

  getByMesPoupanca(mes: number, agencia: number, conta: number): Observable<Historico[]> {
    return this.http.get<Historico[]>(`${this.baseURL}listarPeloMesPoupanca/${mes}/${agencia}/${conta}`);
  }
}
