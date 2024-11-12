import { Component, inject, input, OnInit, output } from '@angular/core';
import { Member } from '../../_models/member';
import { DecimalPipe, NgClass, NgFor, NgIf, NgStyle } from '@angular/common';
import { FileUploader, FileUploadModule } from 'ng2-file-upload';
import { AccountService } from '../../_services/account.service';
import { environment } from '../../../environments/environment';
import { Photo } from '../../_models/photo';
import { MembersService } from '../../_services/members.service';

@Component({
  selector: 'app-photo-editor',
  standalone: true,
  imports: [NgClass , NgFor , NgIf , NgStyle , FileUploadModule , DecimalPipe ],
  templateUrl: './photo-editor.component.html',
  styleUrl: './photo-editor.component.css'
})
export class PhotoEditorComponent implements OnInit {
  private accountservice= inject(AccountService);
  private memberService = inject (MembersService);
  member=input.required<Member>(); //input return value as a signal 
  uploader?:FileUploader;
  hasBaseDropZoneOver=false;
  baseUrl=environment.apiUrl; 
  memberchange=output<Member>();  //member change instance of member interface output return value as a variable
  
  //to get alert when member changes 
  ngOnInit(): void {
    this.initializeUploader();
  }

  
  fileOverBase(e:any){
    this.hasBaseDropZoneOver=e;
  }

  deletePhoto(photo:Photo){
  this.memberService.deletePhoto(photo).subscribe({
    next:_=>{
      const updatedMember={...this.member()};
      updatedMember.photos=updatedMember.photos.filter(x=>x.id!==photo.id); //betgeb el sewar kolaha ela ele b same photoid
      this.memberchange.emit(updatedMember); //7tet updated member f member change
    }
  })
  }
  setMainPhoto(photo:Photo){
    this.memberService.setMainPhoto(photo).subscribe({
      next: _=> { 
        const user=this.accountservice.currentUser();
        if(user){
          user.photoUrl=photo.url;
          this.accountservice.setCurrentUser(user)
        }
        const updatedMember={...this.member()}
        updatedMember.photoUrl=photo.url;
        updatedMember.photos.forEach(p=>{
          if(p.isMain) p.isMain=false;
          if(p.id===photo.id) p.isMain=true;
        });
        this.memberchange.emit(updatedMember);
      }
    })
  }


  //be3rf el person who will uploade the image
  initializeUploader(){
    this.uploader=new FileUploader({
      url:this.baseUrl+'users/add-photo' ,
      authToken : 'Bearer '+ this.accountservice.currentUser()?.token , 
      isHTML5:true , 
      allowedFileType: ['image'] , 
      removeAfterUpload : true , 
      autoUpload : false , 
      maxFileSize : 10*1024*1024 
    });
    this.uploader.onAfterAddingFile=(file)=>{
      file.withCredentials=false
    }
    this.uploader.onSuccessItem=(item , response , status , headers)=>{
      const photo = JSON.parse(response);
      const updatedMember = {...this.member()}
      updatedMember.photos.push(photo);
     this.memberchange.emit(updatedMember); 
     if(photo.isMain){
      const user=this.accountservice.currentUser();
      if(user){
        user.photoUrl=photo.url;
        this.accountservice.setCurrentUser(user)
      }
      updatedMember.photoUrl=photo.url;
      updatedMember.photos.forEach(p=>{
        if(p.isMain) p.isMain=false;
        if(p.id===photo.id) p.isMain=true;
      });
      this.memberchange.emit(updatedMember);
     }
    }
  }
}
