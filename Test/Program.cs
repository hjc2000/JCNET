using JCNET.字符串处理;

string str = @"
	--- 定位、定速运行
	if true then
		--- 设置转速并按此转速运行。
		--- 可以为正负数或 0。设置为正数则正转，负数则反转，0 则停止。
		--- 单位：rpm。
		--- @param value number 浮点数。
		function Servo.SetSpeedAndRun(value)
			-- 要设置的速度如果和当前转速反向了，先断开正转、反转信号。
			if (Servo.Feedback.Speed() * value < 0) then
				Servo.EI.SetForwardSignal(false)
				Servo.EI.SetReverseSignal(false)
			end

			Servo.Param.SetSpeedLimit(value)
			AXIS_SPEED(value)

			if (value > 0) then
				-- 正转
				Servo.EI.SetForwardSignal(true)
				Servo.EI.SetReverseSignal(false)
			elseif (value < 0) then
				-- 反转
				Servo.EI.SetForwardSignal(false)
				Servo.EI.SetReverseSignal(true)
			end
		end

		--- 设置绝对定位立即数并运行。
		--- @param value integer
		function Servo.SetAbsolutePositionAndRun(value)
			print(""SetAbsolutePositionAndRun 定位到"", value)
			AXIS_MOVEABS(value)
			Servo.EI.TriggerRisingEdge(14)
		end

		--- 通过 EI 进行位置预置，设置零点
		function Servo.PresetPosition()
			Servo.EI.TriggerRisingEdge(15)
		end

		--- 取消定位
		function Servo.CancelPositioning()
			AXIS_CANCEL()
		end

		--- 停止转动。
		--- 切换到速度模式，并将速度锁定为 0. 无论原来是什么模式（定位模式、速度模式、转矩模式）都可以使用。
		function Servo.Stop()
			Servo.ChangeToSpeedMode()
			Servo.SetSpeedAndRun(0)
		end
	end
";

int i = 0;
str = str.ReplaceTwoWord("function", "Servo.SetSpeedAndRun", $"G[{i++}] = function");
str = str.ReplaceTwoWord("function", "Servo.SetAbsolutePositionAndRun", $"G[{i++}] = function");
str = str.ReplaceTwoWord("function", "Servo.PresetPosition", $"G[{i++}] = function");
str = str.ReplaceTwoWord("function", "Servo.CancelPositioning", $"G[{i++}] = function");
str = str.ReplaceTwoWord("function", "Servo.Stop", $"G[{i++}] = function");
Console.WriteLine(str);
