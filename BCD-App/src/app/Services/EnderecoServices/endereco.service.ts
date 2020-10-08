import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Endereco } from 'src/app/Models/Endereco';

@Injectable({
  providedIn: 'root'
})
export class EnderecoService {

  baseURL = 'http://localhost:5000/api/endereco/'
  constructor(
    private http: HttpClient
  ) { }

  getEnderecoByCep(cep: number): Observable<Endereco> {
    return this.http.get<Endereco>(`https://viacep.com.br/ws/${cep}/json/`);
  }

  getById(id: number) : Observable<Endereco> {
    return this.http.get<Endereco>(`${this.baseURL}${id}`);
  }

  update(endereco: Endereco) {
    return this.http.put(`${this.baseURL}`,endereco);
  }
  
}
