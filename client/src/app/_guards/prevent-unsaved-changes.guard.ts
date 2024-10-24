import { CanDeactivateFn } from '@angular/router';
import { MemberEditComponent } from '../members/member-edit/member-edit.component';
import { IfStmt } from '@angular/compiler';

export const preventUnsavedChangesGuard: CanDeactivateFn<MemberEditComponent> = (component) => {
if(component.editForm?.dirty){
return confirm('are you sure you want to continue? any changes will be lost');
}
return true;
};
