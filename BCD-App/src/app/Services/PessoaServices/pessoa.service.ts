import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Pessoa } from 'src/app/Models/Pessoa';

@Injectable({
  providedIn: 'root'
})
export class PessoaService {

  baseUrl = 'http://localhost:5000/api/pessoa/';

  constructor(
    private http: HttpClient
  ) { }

  getAllByIdPessoa(idPessoa: number): Observable<Pessoa[]> {
    return this.http.get<Pessoa[]>(`${this.baseUrl}details/${idPessoa}`);
  }
}
