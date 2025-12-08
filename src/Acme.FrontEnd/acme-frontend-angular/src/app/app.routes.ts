import { Routes } from '@angular/router';
import { ProductsComponent } from './pages/products/products.component';
import { UserProfilesComponent } from './pages/user-profiles/user-profiles.component';
import { RecommendationsComponent } from './pages/recommendation/recommendations.component';

export const routes: Routes = [
  { path: '', redirectTo: 'products', pathMatch: 'full' },
  { path: 'products', component: ProductsComponent },
  { path: 'users', component: UserProfilesComponent },
  { path: 'recommendations', component: RecommendationsComponent }
];