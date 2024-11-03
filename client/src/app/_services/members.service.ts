import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { Member } from '../_models/member';
import { of, tap } from 'rxjs';
import { Photo } from '../_models/photo';

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  private http = inject(HttpClient);
  baseUrl = environment.apiUrl;
  members=signal<Member[]>([]);

  getMembers() {
    return this.http.get<Member[]>(this.baseUrl + 'users').subscribe({
      next:members => this.members.set(members)
    })
    
  }

  getMember(username: string) {
    const member = this.members().find(x=>x.username===username);
    if(member!==undefined) return of(member); // to make it observable
    return this.http.get<Member>(this.baseUrl + 'users/' + username); //lw mtl3sh f signal 
  
  }

  updateMember(member:Member){
    return this.http.put(this.baseUrl+'users' ,member).pipe(
      tap(() =>{
        this.members.update(members=>members.map(m=>m.username===member.username ? member:m))
      })
    )
  }  
//that function to update member signal
setMainPhoto(photo:Photo){
  return this.http.put(this.baseUrl+'users/set-main-photo/'+photo.id,{}).pipe(
    tap(()=>{ //tap to make side effect
      this.members.update(members=>members.map(m=>{     // map to loop 
         if(m.photos.includes(photo)){
          m.photoUrl=photo.url
         }
         return m ;
      }))   
    })
  )
}
deletePhoto(photo:Photo){
  return this.http.delete(this.baseUrl+'users/delete-photo/'+photo.id).pipe(
    tap(()=>{ //tap to make side effect
      this.members.update(members=>members.map(m=>{     // map to loop 
         if(m.photos.includes(photo)){
          m.photos=m.photos.filter(x=>x.id !== photo.id)
         }
         return m ;
      }))   
    })
  )
}
}

