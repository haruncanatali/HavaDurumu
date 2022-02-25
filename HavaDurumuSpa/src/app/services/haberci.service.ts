import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Sehir } from 'src/entities/sehir';


@Injectable({
  providedIn: 'root'
})
export class HaberciService {

  pathSehirleriCek_ = "https://localhost:44323/api/values/cities";
  pathSehirGetir_ = "https://localhost:44323/api/values/weather?cityName=";

  constructor(private httpServis:HttpClient) { }

  sehirleriGetir():Observable<Sehir[]>{
    return this.httpServis.get<Sehir[]>(this.pathSehirleriCek_)    
  }

  sehirCek(sehir:string):Observable<Response>{
    var temp = this.pathSehirGetir_+sehir;
    return this.httpServis.get<Response>(temp)
  }
}
