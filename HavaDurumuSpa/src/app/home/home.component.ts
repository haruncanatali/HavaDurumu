import { Component, OnInit } from '@angular/core';
import { Response } from 'src/entities/response';
import { Sehir } from 'src/entities/sehir';
import { HaberciService } from '../services/haberci.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  seciliSehir = "";
  sehirler : Sehir[] = [];
  response_ : Response = new Response();

  constructor(private haberci:HaberciService) {
    this.haberci.sehirleriGetir().subscribe(data=>{
      this.sehirler = data;
    });
   }

   onSelectChange(val:string="") {
      this.response_ = new Response();
      if(val?.length == 0){
        return;
      } 
      this.haberci.sehirCek(val).subscribe((data:any)=>{
         this.response_ = data;
         console.log(this.response_)
      });
   }

  ngOnInit(): void {
  }

}
