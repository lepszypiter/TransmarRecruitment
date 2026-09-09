import { Router } from 'express';
import * as controller from '../controllers/assemblyLineController';
import allocationRoutes from './allocation';

const router = Router();

router.get('/', controller.listAssemblyLines);
router.post('/', controller.createAssemblyLine);
router.get('/:id', controller.getAssemblyLine);
router.put('/:id', controller.updateAssemblyLine);
router.delete('/:id', controller.deleteAssemblyLine);

// Nested routes for allocations: /api/assembly-lines/:id/workstations
router.use('/:id/workstations', allocationRoutes);

export default router;
