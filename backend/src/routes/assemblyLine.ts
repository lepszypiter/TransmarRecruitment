import { Router } from 'express';
import * as controller from '../controllers/assemblyLineController';
import allocationRoutes from './allocation';
import { authenticate } from '../middleware/authMiddleware';

const router = Router();

router.get('/', controller.listAssemblyLines);
router.post('/', authenticate, controller.createAssemblyLine);
router.get('/:id', controller.getAssemblyLine);
router.put('/:id', authenticate, controller.updateAssemblyLine);
router.delete('/:id', authenticate, controller.deleteAssemblyLine);

// Nested routes for allocations: /api/assembly-lines/:id/workstations
router.use('/:id/workstations', allocationRoutes);

export default router;
