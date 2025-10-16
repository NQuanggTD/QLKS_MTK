// Pricing Calculator - Demo Decorator & Strategy Patterns
// File: wwwroot/js/pricing.js

class PricingCalculator {
    constructor() {
        this.baseUrl = '/api/pricing';
        this.init();
    }

    init() {
        console.log('Pricing Calculator initialized with Decorator & Strategy patterns');
    }

    /**
     * Calculate room price with additional services (Decorator Pattern)
     */
    async calculateWithServices(basePrice, services) {
        try {
            const response = await fetch(`${this.baseUrl}/calculate`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ basePrice, services })
            });

            if (!response.ok) throw new Error('Failed to calculate price');
            
            const result = await response.json();
            console.log('✅ Decorator Pattern Result:', result);
            return result;
        } catch (error) {
            console.error('❌ Calculate error:', error);
            showToast('Lỗi tính giá', 'error');
            throw error;
        }
    }

    /**
     * Apply discount strategy (Strategy Pattern)
     */
    async applyDiscount(price, strategyType, parameters = null) {
        try {
            const body = { price, strategyType };
            if (parameters) body.parameters = parameters;

            const response = await fetch(`${this.baseUrl}/discount`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(body)
            });

            if (!response.ok) throw new Error('Failed to apply discount');
            
            const result = await response.json();
            console.log('✅ Strategy Pattern Result:', result);
            return result;
        } catch (error) {
            console.error('❌ Discount error:', error);
            showToast('Lỗi áp dụng giảm giá', 'error');
            throw error;
        }
    }

    /**
     * Get available discount strategies
     */
    async getStrategies() {
        try {
            const response = await fetch(`${this.baseUrl}/strategies`);
            const data = await response.json();
            return data.strategies;
        } catch (error) {
            console.error('❌ Get strategies error:', error);
            return [];
        }
    }

    /**
     * Get available additional services
     */
    async getServices() {
        try {
            const response = await fetch(`${this.baseUrl}/services`);
            const data = await response.json();
            return data.services;
        } catch (error) {
            console.error('❌ Get services error:', error);
            return [];
        }
    }

    /**
     * Calculate full booking price (Decorator + Strategy combined)
     */
    async calculateFullPrice(basePrice, services, strategyType, strategyParams = null) {
        try {
            // Step 1: Apply Decorator Pattern
            const decoratorResult = await this.calculateWithServices(basePrice, services);
            const priceWithServices = decoratorResult.finalPrice;

            // Step 2: Apply Strategy Pattern
            const strategyResult = await this.applyDiscount(
                priceWithServices, 
                strategyType, 
                strategyParams
            );

            return {
                basePrice: basePrice,
                priceWithServices: priceWithServices,
                finalPrice: strategyResult.discountedPrice,
                totalSaved: basePrice + decoratorResult.addedCost - strategyResult.discountedPrice,
                serviceDescription: decoratorResult.description,
                discountDescription: strategyResult.description,
                breakdown: {
                    base: basePrice,
                    servicesAdded: decoratorResult.addedCost,
                    subtotal: priceWithServices,
                    discountAmount: strategyResult.savedAmount,
                    final: strategyResult.discountedPrice
                }
            };
        } catch (error) {
            console.error('❌ Calculate full price error:', error);
            throw error;
        }
    }
}

// Demo functions for testing in browser console
window.pricingDemo = {
    calculator: new PricingCalculator(),

    /**
     * Demo 1: Decorator Pattern - Adding services
     */
    async demoDecorator() {
        console.log('🎨 DECORATOR PATTERN DEMO');
        console.log('='.repeat(50));

        const basePrice = 1000000;
        
        // Test 1: Basic room
        console.log('\n1️⃣ Basic room only:');
        await this.calculator.calculateWithServices(basePrice, []);

        // Test 2: Room + Breakfast
        console.log('\n2️⃣ Room + Breakfast:');
        await this.calculator.calculateWithServices(basePrice, ['breakfast']);

        // Test 3: Room + Breakfast + Spa
        console.log('\n3️⃣ Room + Breakfast + Spa:');
        await this.calculator.calculateWithServices(basePrice, ['breakfast', 'spa']);

        // Test 4: Full package
        console.log('\n4️⃣ Full package:');
        await this.calculator.calculateWithServices(basePrice, ['breakfast', 'spa', 'airport', 'laundry']);
    },

    /**
     * Demo 2: Strategy Pattern - Applying discounts
     */
    async demoStrategy() {
        console.log('\n🎯 STRATEGY PATTERN DEMO');
        console.log('='.repeat(50));

        const price = 2000000;

        // Test 1: No discount
        console.log('\n1️⃣ No discount:');
        await this.calculator.applyDiscount(price, 'none');

        // Test 2: VIP discount
        console.log('\n2️⃣ VIP discount:');
        await this.calculator.applyDiscount(price, 'vip');

        // Test 3: Loyalty discount
        console.log('\n3️⃣ Loyalty discount (8 visits):');
        await this.calculator.applyDiscount(price, 'loyalty', { visitCount: 8 });

        // Test 4: Early bird discount
        console.log('\n4️⃣ Early Bird discount (25 days):');
        await this.calculator.applyDiscount(price, 'earlybird', { daysInAdvance: 25 });
    },

    /**
     * Demo 3: Combined - Decorator + Strategy
     */
    async demoCombined() {
        console.log('\n🚀 COMBINED PATTERNS DEMO');
        console.log('='.repeat(50));

        console.log('\n📝 Scenario: VIP customer booking Deluxe room with full services');
        
        const result = await this.calculator.calculateFullPrice(
            1500000,                                    // Base price
            ['breakfast', 'spa', 'airport', 'laundry'], // Services
            'vip'                                       // Discount strategy
        );

        console.log('\n💰 Final Calculation:');
        console.log(`  Base Price:        ${formatCurrency(result.breakdown.base)}`);
        console.log(`  Services Added:  + ${formatCurrency(result.breakdown.servicesAdded)}`);
        console.log(`  Subtotal:          ${formatCurrency(result.breakdown.subtotal)}`);
        console.log(`  VIP Discount:    - ${formatCurrency(result.breakdown.discountAmount)}`);
        console.log(`  ────────────────────────────────`);
        console.log(`  FINAL PRICE:       ${formatCurrency(result.breakdown.final)} ⭐`);
        console.log(`\n  Services: ${result.serviceDescription}`);
        console.log(`  Discount: ${result.discountDescription}`);
    },

    /**
     * Demo 4: All available options
     */
    async demoOptions() {
        console.log('\n📋 AVAILABLE OPTIONS');
        console.log('='.repeat(50));

        console.log('\n🛎️ Available Services:');
        const services = await this.calculator.getServices();
        services.forEach(s => {
            console.log(`  • ${s.displayName} (${s.name}): ${formatCurrency(s.price)}`);
        });

        console.log('\n🎫 Available Discount Strategies:');
        const strategies = await this.calculator.getStrategies();
        strategies.forEach(s => {
            console.log(`  • ${s.displayName} (${s.name}): ${s.description}`);
        });
    },

    /**
     * Run all demos
     */
    async runAll() {
        console.clear();
        console.log('🎉 HOTEL NQ - DESIGN PATTERNS DEMO');
        console.log('='.repeat(50));
        
        await this.demoDecorator();
        await new Promise(resolve => setTimeout(resolve, 1000));
        
        await this.demoStrategy();
        await new Promise(resolve => setTimeout(resolve, 1000));
        
        await this.demoCombined();
        await new Promise(resolve => setTimeout(resolve, 1000));
        
        await this.demoOptions();
        
        console.log('\n✅ All demos completed!');
        console.log('💡 Try: pricingDemo.calculator.calculateFullPrice(1500000, ["breakfast", "spa"], "vip")');
    }
};

// Helper function
function formatCurrency(amount) {
    return new Intl.NumberFormat('vi-VN', {
        style: 'currency',
        currency: 'VND'
    }).format(amount);
}

// Auto-run demo on load (commented out by default)
// document.addEventListener('DOMContentLoaded', () => {
//     console.log('💡 Pricing Calculator loaded! Try:');
//     console.log('  • pricingDemo.runAll() - Run all demos');
//     console.log('  • pricingDemo.demoDecorator() - Decorator pattern only');
//     console.log('  • pricingDemo.demoStrategy() - Strategy pattern only');
//     console.log('  • pricingDemo.demoCombined() - Combined patterns');
// });
