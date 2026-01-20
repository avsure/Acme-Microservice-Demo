import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { ProductService, Product, CreateProductRequest } from '../../services/product.service';

@Component({
  standalone: true,
  imports: [CommonModule, FormsModule],
  selector: 'app-products',
  templateUrl: './products.component.html'
})
export class ProductsComponent implements OnInit {

  products: Product[] = [];
  loading = true;

  newProduct: CreateProductRequest = {
    name: '',
    price: 0
  };

  constructor(
    private productService: ProductService,
    private router: Router
  ) {}

  ngOnInit() {
    this.loadProducts();
  }

  loadProducts(){
    this.productService.getAll().subscribe({
      next: (p) => {
        this.products = p;
        this.loading = false;
      },
      error: (err) => {
        console.error('Error fetching products', err);
        this.loading = false;
      }
    });
  }

  createProduct(){
    this.productService.create(this.newProduct).subscribe(()=>{
      this.newProduct = { name:'', price:0 };
      this.loadProducts();
    });
  }

  viewRecommendations(productId: string){
    this.router.navigate(['/recommendations'], {
      queryParams: { productId }
    });
  }

  trackById(index: number, item: Product){
    return item.id;
  }
}
