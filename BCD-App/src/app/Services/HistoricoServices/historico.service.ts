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

  getByMes(mes: number): Observable<Historico[]> {
    return this.http.get<Historico[]>(`${this.baseURL}listarPeloMes/${mes}`);
  }
}
