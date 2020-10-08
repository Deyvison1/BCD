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

  getById(id: number) : Observable<Pessoa> {
    return this.http.get<Pessoa>(`${this.baseUrl}byId/${id}`);
  }

  getByIdList(id: number): Observable<Pessoa[]> {
    return this.http.get<Pessoa[]>(`${this.baseUrl}${id}`);
  }

  getSearchNomeOrCpf(search: string): Observable<Pessoa[]> {
    return this.http.get<Pessoa[]>(`${this.baseUrl}buscar/${search}`);
  }

  getAll(): Observable<Pessoa[]> {
    return this.http.get<Pessoa[]>(this.baseUrl);
  }

  getAllByIdPessoa(idPessoa: number): Observable<Pessoa[]> {
    return this.http.get<Pessoa[]>(`${this.baseUrl}details/${idPessoa}`);
  }

  update(pessoa: Pessoa) {
    return this.http.put(`${this.baseUrl}`, pessoa);
  }
}
