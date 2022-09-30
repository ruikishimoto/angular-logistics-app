import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { MessageService } from 'primeng/api';
import { CreateEmployee, Role, User, UserIdentity } from '@shared/models';
import { ApiService } from '@shared/services';
import { UserRole } from '@shared/types';

@Component({
  selector: 'app-add-employee',
  templateUrl: './add-employee.component.html',
  styleUrls: ['./add-employee.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class AddEmployeeComponent implements OnInit {
  private userRoles: string | string[];

  public suggestedUsers: User[];
  public form: FormGroup;
  public roles: Role[];
  public loading: boolean;

  constructor(
    private apiService: ApiService,
    private messageService: MessageService,
    private oidcService: OidcSecurityService) 
  {
    this.suggestedUsers = [];
    this.roles = [];
    this.userRoles = [];
    this.loading = false;

    this.form = new FormGroup({
      user: new FormControl('', Validators.required),
      role: new FormControl('', Validators.required),
    });
  }

  public ngOnInit(): void {
    this.fetchRoles();
    this.oidcService.getUserData().subscribe((userData: UserIdentity) => this.userRoles = userData.role!);
  }

  public searchUser(event: any) {
    this.apiService.getUsers(event.query).subscribe(result => {
      if (result.success && result.items) {
        this.suggestedUsers = result.items;
      }
    });
  }

  public clearSelctedRole() {
    this.form.patchValue({
      role: {name: '', displayName: ' '}
    });
  }

  public submit() {
    const user = this.form.value.user as User;
    
    if (!user) {
      this.messageService.add({key: 'notification', severity: 'error', summary: 'Error', detail: 'Select user'});
      return;
    }

    const newEmployee: CreateEmployee = {
      id: user.id!,
      role: this.form.value.role
    };

    this.loading = true;
    this.apiService.createEmployee(newEmployee).subscribe(result => {
      if (result.success) {
        this.messageService.add({key: 'notification', severity: 'success', summary: 'Notification', detail: 'New employee has been added successfully'});
        this.form.reset();
      }

      this.loading = false;
    });
  }

  private fetchRoles() {
    this.loading = true;

    this.apiService.getRoles().subscribe(result => {
      if (result.success && result.items) {
        const roles = result.items;
        const roleNames = roles.map(i => i.name);
        
        if (this.userRoles.includes(UserRole.Owner)) {
          roles.splice(roleNames.indexOf(UserRole.Owner), 1);
        }
        else if (this.userRoles.includes(UserRole.Manager)) {
          roles.splice(roleNames.indexOf(UserRole.Owner), 1);
          roles.splice(roleNames.indexOf(UserRole.Manager), 1);
        }
        
        this.roles.push({name: '', displayName: ' '});
        this.roles.push(...roles);
      }

      this.loading = false;
    });
  }
}
