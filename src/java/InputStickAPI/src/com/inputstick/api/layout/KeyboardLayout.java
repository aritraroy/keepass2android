package com.inputstick.api.layout;

import com.inputstick.api.ConnectionManager;
import com.inputstick.api.basic.InputStickHID;
import com.inputstick.api.hid.HIDKeycodes;
import com.inputstick.api.hid.HIDTransaction;
import com.inputstick.api.hid.KeyboardReport;

public abstract class KeyboardLayout {
	
	public static final int MAX_SCANCODE = 0x60;
	
	public static final byte[] scanCodeToHID = {
		/* 0x00 */ 0,
		/* 0x01 */ HIDKeycodes.KEY_ESCAPE,
		/* 0x02 */ HIDKeycodes.KEY_1,
		/* 0x03 */ HIDKeycodes.KEY_2,
		/* 0x04 */ HIDKeycodes.KEY_3,
		/* 0x05 */ HIDKeycodes.KEY_4,
		/* 0x06 */ HIDKeycodes.KEY_5,
		/* 0x07 */ HIDKeycodes.KEY_6,
		/* 0x08 */ HIDKeycodes.KEY_7,
		/* 0x09 */ HIDKeycodes.KEY_8,
		/* 0x0a */ HIDKeycodes.KEY_9,
		/* 0x0b */ HIDKeycodes.KEY_0,
		/* 0x0c */ HIDKeycodes.KEY_MINUS,
		/* 0x0d */ HIDKeycodes.KEY_EQUALS,
		/* 0x0e */ HIDKeycodes.KEY_BACKSPACE,
		/* 0x0f */ HIDKeycodes.KEY_TAB,
			
		
		/* 0x10 */ HIDKeycodes.KEY_Q,
		/* 0x11 */ HIDKeycodes.KEY_W,
		/* 0x12 */ HIDKeycodes.KEY_E,
		/* 0x13 */ HIDKeycodes.KEY_R,
		/* 0x14 */ HIDKeycodes.KEY_T,
		/* 0x15 */ HIDKeycodes.KEY_Y,
		/* 0x16 */ HIDKeycodes.KEY_U,
		/* 0x17 */ HIDKeycodes.KEY_I,
		/* 0x18 */ HIDKeycodes.KEY_O,
		/* 0x19 */ HIDKeycodes.KEY_P,
		/* 0x1a */ HIDKeycodes.KEY_LEFT_BRACKET,
		/* 0x1b */ HIDKeycodes.KEY_RIGHT_BRACKET,
		/* 0x1c */ HIDKeycodes.KEY_ENTER,
		/* 0x1d */ 0, //RL CTRL
		/* 0x1e */ HIDKeycodes.KEY_A,
		/* 0x1f */ HIDKeycodes.KEY_S,	
		
		/* 0x20 */ HIDKeycodes.KEY_D,
		/* 0x21 */ HIDKeycodes.KEY_F,
		/* 0x22 */ HIDKeycodes.KEY_G,
		/* 0x23 */ HIDKeycodes.KEY_H,
		/* 0x24 */ HIDKeycodes.KEY_J,
		/* 0x25 */ HIDKeycodes.KEY_K,
		/* 0x26 */ HIDKeycodes.KEY_L,
		/* 0x27 */ HIDKeycodes.KEY_SEMICOLON,
		/* 0x28 */ HIDKeycodes.KEY_APOSTROPHE,
		/* 0x29 */ HIDKeycodes.KEY_GRAVE,
		/* 0x2a */ 0, //L SHIFT
		/* 0x2b */ HIDKeycodes.KEY_BACKSLASH,
		/* 0x2c */ HIDKeycodes.KEY_Z,
		/* 0x2d */ HIDKeycodes.KEY_X,
		/* 0x2e */ HIDKeycodes.KEY_C,
		/* 0x2f */ HIDKeycodes.KEY_V,		
		
		/* 0x30 */ HIDKeycodes.KEY_B,
		/* 0x31 */ HIDKeycodes.KEY_N,
		/* 0x32 */ HIDKeycodes.KEY_M,
		/* 0x33 */ HIDKeycodes.KEY_COMA,
		/* 0x34 */ HIDKeycodes.KEY_DOT,
		/* 0x35 */ HIDKeycodes.KEY_SLASH,
		/* 0x36 */ 0, //R SHIFT
		/* 0x37 */ HIDKeycodes.KEY_PRINT_SCREEN,
		/* 0x38 */ 0, //RL ALT
		/* 0x39 */ HIDKeycodes.KEY_SPACEBAR,
		/* 0x3a */ HIDKeycodes.KEY_CAPS_LOCK,
		/* 0x3b */ HIDKeycodes.KEY_F1,
		/* 0x3c */ HIDKeycodes.KEY_F2,
		/* 0x3d */ HIDKeycodes.KEY_F3,
		/* 0x3e */ HIDKeycodes.KEY_F4,
		/* 0x3f */ HIDKeycodes.KEY_F5,
		
		/* 0x40 */ HIDKeycodes.KEY_F6,
		/* 0x41 */ HIDKeycodes.KEY_F7,
		/* 0x42 */ HIDKeycodes.KEY_F8,
		/* 0x43 */ HIDKeycodes.KEY_F9,
		/* 0x44 */ HIDKeycodes.KEY_F10,
		/* 0x45 */ HIDKeycodes.KEY_NUM_LOCK,
		/* 0x46 */ HIDKeycodes.KEY_SCROLL_LOCK,
		/* 0x47 */ HIDKeycodes.KEY_HOME,
		/* 0x48 */ HIDKeycodes.KEY_ARROW_UP,
		/* 0x49 */ HIDKeycodes.KEY_PAGE_UP,
		/* 0x4a */ 0, //-
		/* 0x4b */ HIDKeycodes.KEY_ARROW_LEFT,
		/* 0x4c */ 0, //CENTER
		/* 0x4d */ HIDKeycodes.KEY_ARROW_RIGHT,
		/* 0x4e */ 0, //+
		/* 0x4f */ HIDKeycodes.KEY_END,
		
		/* 0x50 */ HIDKeycodes.KEY_ARROW_DOWN,
		/* 0x51 */ HIDKeycodes.KEY_PAGE_DOWN,
		/* 0x52 */ HIDKeycodes.KEY_INSERT,
		/* 0x53 */ HIDKeycodes.KEY_DELETE,
		/* 0x54 */ 0,
		/* 0x55 */ 0,
		/* 0x56 */ HIDKeycodes.KEY_BACKSLASH_NON_US,  //GERMAN LAYOUT!
		/* 0x57 */ HIDKeycodes.KEY_F11,
		/* 0x58 */ HIDKeycodes.KEY_F12,
		/* 0x59 */ 0,
		/* 0x5a */ 0,
		/* 0x5b */ 0,
		/* 0x5c */ 0,
		/* 0x5d */ 0,
		/* 0x5e */ 0,
		/* 0x5f */ 0,		
		
	};	
	
	public static final int LAYOUT_CODE = 0;
	
	public abstract int[][] getLUT();
	public abstract int[][] getDeadkeyLUT();
	public abstract int[] 	getDeadkeys();
	public abstract String getLocaleName();
	public abstract void type(String text);
	public abstract void type(String text, byte modifiers);
	public abstract char getChar(int scanCode, boolean capsLock, boolean shift, boolean altGr);
	
	public void type(int[][] lut, int[][] deadkeyLUT, int[] deadkeys, String text, byte modifiers) {
		if (InputStickHID.getState() == ConnectionManager.STATE_READY) {			
			char[] chars = text.toCharArray();
			HIDTransaction t;
			for (char c : chars) {
				t = getHIDTransaction(lut, deadkeyLUT, deadkeys, c, modifiers);				
				if (t != null) {
					InputStickHID.addKeyboardTransaction(t);
				}
			}
		}
	}	
	
	public static int hidToScanCode(byte key) {
		for (int scanCode = 0; scanCode < MAX_SCANCODE; scanCode++) {
			if (scanCodeToHID[scanCode] == key) {
				return scanCode;
			}
		}
		return -1;
	}
	
	public static char getChar(int[][] lut, int scanCode, boolean capsLock, boolean shift, boolean altGr) {
		if ((scanCode >= MAX_SCANCODE) || (scanCode < 0)) {
			return (char)0;
		}
		
		int index = 1;
		
		if ((capsLock) && (lut[scanCode][0] > 0)) {
			//capslock is on and it affects current key						
			if (lut[scanCode][0] == 1) {
				if (shift) {
					index = 1; //caps + shift = default
				} else {
					index = 2; //shift
				}
			} else {
				// >1
				if (shift) {
					if (altGr) {
						index = 4; //caps + shift + alt = alt
					} else {					
						index = 1; //caps + shift = default
					}
				} else {
					if (altGr) {
						index = 5; //caps + alt = shift + alt
					} else {
						index = 2; //caps = shift
					}
				}					
			}
		} else {				
			if (shift) {
				index = 2;
			}		
			if (altGr) {
				if (shift) {
					index = 5;
				} else {
					index = 4;
				}
			} 
		}
		
		if (lut[scanCode][index] == -1) {
			index = 1;
		} 
		return (char)lut[scanCode][index];		
	}
	
	public static int getScanCode(int[][] lut, char c) {		
		for (int scanCode = 0; scanCode < MAX_SCANCODE; scanCode++) {
			if (lut[scanCode][0] == -1) {
				continue;
			} else {
				for (int i = 1; i < 6; i++) {
					if (lut[scanCode][i] == (int)c) {
						return scanCode;
					}
				}
			}
		}
		return -1;
	}
	
	public static byte getKey(int scanCode) {	
		return scanCodeToHID[scanCode];
	}
	
	public static byte getModifiers(int[][] lut, int scanCode, char c) {
		if (lut[scanCode][1] == (int)c) {
			return 0;
		}
		if (lut[scanCode][2] == (int)c) {
			return HIDKeycodes.SHIFT_LEFT;
		}
		if (lut[scanCode][3] == (int)c) {
			return HIDKeycodes.CTRL_LEFT;
		}
		if (lut[scanCode][4] == (int)c) {
			return HIDKeycodes.ALT_RIGHT;
		}
		if (lut[scanCode][5] == (int)c) {
			return HIDKeycodes.SHIFT_LEFT | HIDKeycodes.ALT_RIGHT;
		}
		
		return 0;
	}
	
	
	public static boolean isDeadkey(int[] deadkeys, char c) {
		if (deadkeys != null) {
			for (int key : deadkeys) {
				if (key == (int)c) {
					return true;
				}
			}
		}
		return false;
	}
	
	public static int searchLUT(int[][] deadkeyLUT, char c, int returnIndex) {
		if (deadkeyLUT != null) {
			for (int i = 0; i < deadkeyLUT.length; i++) {
				if (deadkeyLUT[i][2] == (int)c) {
					return deadkeyLUT[i][returnIndex];
				}
			}
		}
		return -1;
	}
	
	public static int findDeadKey(int[][] deadkeyLUT, char c) {
		return searchLUT(deadkeyLUT, c, 0);
	}
	
	public static int findFollowingKey(int[][] deadkeyLUT, char c) {
		return searchLUT(deadkeyLUT, c, 1);
	}
	
	public static HIDTransaction getHIDTransaction(int[][] lut, int[][] deadkeyLUT, int[] deadkeys, char c, byte additionalModifierKeys) {
		byte modifiers, key;
		int scanCode;
		
		HIDTransaction t = new HIDTransaction();		
		scanCode = getScanCode(lut, c);
		if (scanCode > 0) {			
			key = getKey(scanCode);
			modifiers = getModifiers(lut, scanCode, c);
			modifiers |= additionalModifierKeys;
			
			t.addReport(new KeyboardReport(modifiers, (byte)0));
			t.addReport(new KeyboardReport(modifiers, key));
			t.addReport(new KeyboardReport());
			
			//add space after deadkey!
			if (isDeadkey(deadkeys, c)) {
				t.addReport(new KeyboardReport((byte)0, HIDKeycodes.KEY_SPACEBAR)); //this won't work if modifiers are present!
				t.addReport(new KeyboardReport());
			}
			
		} else {
			//check if character can be obtained using deadkey:
			int deadkey = findDeadKey(deadkeyLUT, c);
			if (deadkey > 0) { 				
				//yes it can
				int following = findFollowingKey(deadkeyLUT, c);								
				
				scanCode = getScanCode(lut, (char)deadkey);
				key = getKey(scanCode);
				modifiers = getModifiers(lut, scanCode, (char)deadkey);
				t.addReport(new KeyboardReport(modifiers, (byte)0));
				t.addReport(new KeyboardReport(modifiers, key));
				t.addReport(new KeyboardReport());
				
				scanCode = getScanCode(lut, (char)following);
				key = getKey(scanCode);
				modifiers = getModifiers(lut, scanCode, (char)following);
				t.addReport(new KeyboardReport(modifiers, (byte)0));
				t.addReport(new KeyboardReport(modifiers, key));
				t.addReport(new KeyboardReport());
			}
			
		}
		return t;
	}		
	
	public static KeyboardLayout getLayout(String locale) {
		if (locale != null) {
			if (locale.equals(UnitedStatesLayout.getInstance().getLocaleName())) {
				return UnitedStatesLayout.getInstance();
			} else if (locale.equals(PolishLayout.getInstance().getLocaleName())) {
				return PolishLayout.getInstance();
			} else if (locale.equals(RussianLayout.getInstance().getLocaleName())) {
				return RussianLayout.getInstance();
			} else if (locale.equals(GermanLayout.getInstance().getLocaleName())) {
				return GermanLayout.getInstance();
			} else if (locale.equals(SlovakLayout.getInstance().getLocaleName())) {
				return SlovakLayout.getInstance();
			} else if (locale.equals(PortugueseBrazilianLayout.getInstance().getLocaleName())) {
				return PortugueseBrazilianLayout.getInstance();
			} else if (locale.equals(DvorakLayout.getInstance().getLocaleName())) {
				return DvorakLayout.getInstance();
			} else if (locale.equals(NorwegianLayout.getInstance().getLocaleName())) {
				return NorwegianLayout.getInstance();
			} else if (locale.equals(SwedishLayout.getInstance().getLocaleName())) {
				return SwedishLayout.getInstance();
			} else if (locale.equals(FrenchLayout.getInstance().getLocaleName())) {
				return FrenchLayout.getInstance();
			} else if (locale.equals(SpanishLayout.getInstance().getLocaleName())) {
				return SpanishLayout.getInstance();
			} else if (locale.equals(UnitedKingdomLayout.getInstance().getLocaleName())) {
				return UnitedKingdomLayout.getInstance();
			} else if (locale.equals(GermanMacLayout.getInstance().getLocaleName())) {
				return GermanMacLayout.getInstance(); // TODO
			} else if (locale.equals(ItalianLayout.getInstance().getLocaleName())) {
				return ItalianLayout.getInstance();
			} else if (locale.equals(FinnishLayout.getInstance().getLocaleName())) {
				return FinnishLayout.getInstance();
			} else if (locale.equals(SwissFrenchLayout.getInstance().getLocaleName())) {
				return SwissFrenchLayout.getInstance();
			} else if (locale.equals(SwissGermanLayout.getInstance().getLocaleName())) {
				return SwissGermanLayout.getInstance();
			} else if (locale.equals(HebrewLayout.getInstance().getLocaleName())) {
				return HebrewLayout.getInstance();
			} else if (locale.equals(DanishLayout.getInstance().getLocaleName())) {
				return DanishLayout.getInstance();
			}													
			
		}

		return UnitedStatesLayout.getInstance();
	}
	
}
