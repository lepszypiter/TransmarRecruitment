import { Router } from 'express';
import * as controller from '../controllers/productController';
import { authenticate } from '../middleware/authMiddleware';

const router = Router();

router.get('/', controller.listProducts);
router.post('/', authenticate, controller.createProduct);
router.get('/:id', controller.getProduct);
router.put('/:id', authenticate, controller.updateProduct);
router.delete('/:id', authenticate, controller.deleteProduct);

export default router;
